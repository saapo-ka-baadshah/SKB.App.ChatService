namespace SKB.App.ChatService.Core.Options;

/// <summary>
/// Options for the Ollama API
/// </summary>
public class OllamaOptions
{
	/// <summary>
	/// OllamaOptions Key
	/// </summary>
	public static string Key { get; set; } = nameof(OllamaOptions);

	/// <summary>
	/// Ollama host
	/// </summary>
	public required string Host { get; init; } = "localhost";

	/// <summary>
	/// Ollama model
	/// </summary>
	public required string Model { get; init; } = "llama3.1:8b";

	/// <summary>
	/// Ollama port
	/// </summary>
	public required int Port { get; init; } = 11443;

	/// <summary>
	/// Ollama transport protocol
	/// </summary>
	public string TransportProtocol { get; init; } = "tcp";

	/// <summary>
	/// Ollama transport communication protocol
	/// </summary>
	public string TransportCommunictaionProtocol { get; init; } = "http";

	/// <summary>
	/// OllamaSharp if ssl/tls is enabled
	/// </summary>
	public bool Ssl { get; init; }

	/// <summary>
	/// Ssl certificate for OllamaSharp
	/// </summary>
	public object? SslCertificate { get; init; }

	/// <summary>
	/// Provides host uri for the OllamaSharp client
	/// </summary>
	/// <returns>Host uri <see cref="Uri"/> </returns>
	public Uri GetHostUri()
	{
		string commProtocol = Ssl ? "https" : "http";
		return new Uri($"{commProtocol}://{Host}:{Port}");
	}

	/// <summary>
	/// Dictates how OllamaOptions should be rendered as string
	/// </summary>
	/// <returns>Host uri string</returns>
	public override string ToString()
	{
		return GetHostUri().ToString();
	}
}
