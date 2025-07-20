namespace SKB.App.ChatService.Abstractions.Events;

/// <summary>
/// Base chat event that consists of an object type
/// </summary>
public abstract class ChatEventBase: IChatEvent
{
	/// <summary>
	/// A handling object to be processed by the chat service
	/// </summary>
	public object? HandlingObject { get; set; }

	/// <summary>
	/// A prompt list for the chat service to process
	/// </summary>
	public required List<string> Prompts { get; set; }
}
