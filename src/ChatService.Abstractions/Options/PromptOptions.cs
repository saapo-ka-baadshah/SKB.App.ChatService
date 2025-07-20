namespace SKB.App.ChatService.Abstractions.Options;

/// <summary>
/// Options for loosely coupled prompt options
/// </summary>
public class PromptOptions
{
	/// <summary>
	/// System chat descriptions for AI chat model
	/// </summary>
	public List<string>? SystemChatPromptList {get; set;} = [
		"Your name is SKB.ChatAgent.",
		"Your task is to perform operations based on the system states.",
		"You will be provided with multiple Error texts occuring in the operational architecture.",
		"You supposed to generate strings that look like log statements"
	];

	/// <summary>
	/// Provides prompts for McpTool with the instructions, this is to be injected
	/// </summary>
	public List<string>? McpToolInstructionPrompt { get; set; } =
	[
		"Your task is to invoke the MCP tool, only if you are provided any MCP tools by the MCP server.",
		"You are allowed to use multiple MCP tools.",
	];

	/// <summary>
	/// Default chat prompts which should be added in the chat list with every message sent to the chat service
	/// </summary>
	public List<string>? DefaultUserChatPromptList { get; set; } = [];
}
