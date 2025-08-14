using System.Globalization;
using MassTransit;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using SKB.App.ChatService.Abstractions.Events;
using SKB.App.ChatService.Abstractions.Options;
using SKB.App.ChatService.EventConsumers.Extensions;

namespace SKB.App.ChatService.EventConsumers;

/// <summary>
/// Chat event consumer of a generic type
/// </summary>
public class GenericChatConsumer: IConsumer<GenericChatEvent>
{
	private readonly ILogger<GenericChatConsumer> _logger;
	private readonly IChatClient _chatClient;
	private readonly IList<McpClientTool> _mcpClientTools;
	private readonly PromptOptions _promptOptions;

	/// <summary>
	/// Consumes a generic chat event.
	/// </summary>
	/// <param name="chatClient">AI chat client.</param>
	/// <param name="mcpClientTools">Injected MCP tools from MCP client</param>
	/// <param name="logger">Injected logger</param>
	/// <param name="options">Injected Options</param>
	public GenericChatConsumer(
		IChatClient chatClient,
		IList<McpClientTool>? mcpClientTools,
		ILogger<GenericChatConsumer> logger,
		IOptions<PromptOptions> options)
	{
		_logger = logger;
		_chatClient = chatClient;
		_mcpClientTools = mcpClientTools ?? new List<McpClientTool>();
		_promptOptions = options.Value;
	}

	/// <summary>
	/// Consumes a generic chat event. Alternate constructor for non MCP server injections
	/// </summary>
	/// <param name="chatClient">AI chat client.</param>
	/// <param name="logger">Injected logger</param>
	/// <param name="options">Injected Options</param>
	public GenericChatConsumer(
		IChatClient chatClient,
		ILogger<GenericChatConsumer> logger,
		IOptions<PromptOptions> options)
	{
		_logger = logger;
		_chatClient = chatClient;
		_mcpClientTools = new List<McpClientTool>();
		_promptOptions = options.Value;
	}

	/// <summary>
	/// Consume the event of implementation for IChat
	/// </summary>
	/// <param name="context">MassTransit consumer context</param>
	/// <returns></returns>
	public async Task Consume(ConsumeContext<GenericChatEvent> context)
	{
		var chatEvent = context.Message;
		if (chatEvent.Prompts.Count == 0)
		{
			_logger.WarnAi("No prompts provided! Please provide at least one prompt.");
			return;
		}

		List<ChatMessage> messages = [];
		messages.AddRange(
			_promptOptions
				.SystemChatPromptList!
				.Select(systemPrompt => new ChatMessage(ChatRole.System, systemPrompt)));
		// Add system prompts to chat service
		// Add default user prompts
		messages.AddRange(
			_promptOptions
				.DefaultUserChatPromptList!
				.Select(defautlUserPrompt => new ChatMessage(ChatRole.User, defautlUserPrompt)));

		if (_mcpClientTools.Count > 0)
		{
			messages.AddRange(
				_promptOptions
					.McpToolInstructionPrompt!
					.Select(mcpToolPrompt =>  new ChatMessage(ChatRole.System, mcpToolPrompt))
				);

			foreach (var mcpTool in _mcpClientTools)
			{
				messages.Add(
					new ChatMessage(
						ChatRole.System,
						$"Tool: {mcpTool.Name}, " +
						$"Description: {mcpTool.Description}, " +
						$"JsonSchema: {mcpTool.JsonSchema}")
					);
			}

		}

		// Since the prompt is provided, add that to the message chain
		messages.AddRange(
			chatEvent
				.Prompts
				.Select(chatPrompt => new ChatMessage(ChatRole.User, chatPrompt)));

		// Add HandlingObject to the chat context
		if (chatEvent.HandlingObject is not null)
		{
			try
			{
				messages.Add(
					new ChatMessage(ChatRole.User, chatEvent.HandlingObject.ToString())
				);
			}
			catch (Exception e)
			{
				_logger.WarnAi("HandlingObject cannot be processed as string {}", chatEvent.HandlingObject);
				_logger.WarnAi("Captured an error: {}", e);
			}
		}



		// Create default chat options
		ChatOptions chatOptions = _mcpClientTools.Count > 0
			? new ChatOptions()
				{
					Tools = [.. _mcpClientTools]
				}
			: new ChatOptions();

		ChatResponse chatResponse = await _chatClient.GetResponseAsync(
				messages: messages,
				options: chatOptions
			);

		_logger.LogAi(
				"{timestamp} Chat response: {chatResponse}",
				DateTime.Now.ToString(CultureInfo.InvariantCulture),chatResponse);
	}
}
