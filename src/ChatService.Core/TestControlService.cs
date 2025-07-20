using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;

namespace SKB.App.ChatService.Core;

/// <summary>
/// Test control service that adds a console based chatbot
/// </summary>
public class TestControlService(
		IChatClient chatClient,
		IList<McpClientTool> mcpClientTools): BackgroundService
{
	/// <summary>
	/// Execute service in the background
	/// </summary>
	/// <param name="stoppingToken">Stopping token for cancellation</param>
	/// <returns></returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			List<ChatMessage> messages =
			[
				new (ChatRole.System, "Your task is to invoke the MCP tool")
			];
			System.Console.Write("You: ");
			string? messageString = System.Console.ReadLine();
			messages.Add(new (ChatRole.User, messageString!));

			ChatOptions chatOptions = new()
			{
				Tools = [.. mcpClientTools]
			};

			System.Console.Write("Assistant: ");

			await foreach (var stream in
			               chatClient.GetStreamingResponseAsync(
				               messages,
				               chatOptions,
				               stoppingToken))
				System.Console.Write(stream);

			System.Console.WriteLine("");
		}
	}
}
