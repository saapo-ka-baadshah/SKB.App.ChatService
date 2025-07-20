using Microsoft.Extensions.Logging;

namespace SKB.App.ChatService.EventConsumers.Extensions;

/// <summary>
/// Adds log extensions for the Loggers referred with the AI
/// </summary>
public static class AiLogExtensions
{
	/// <summary>
	/// Adds a method to log a response from the AI Chat service
	/// </summary>
	/// <param name="logger">extension for ilogger</param>
	/// <param name="logString">string for the structured logger</param>
	/// <param name="structuredLogArgs">structured log parameters for the structured strings</param>
	public static void LogAi(this ILogger logger, string logString, params object[] structuredLogArgs)
	{
		using (logger.BeginScope(
				new Dictionary<string, object>()
				{
					["LogType"] = "AiLog",
					["LogRootGenerator"] = "Ollama"
				}
			))
		{
			logger.LogInformation(logString, structuredLogArgs);
		}
	}

	/// <summary>
	/// Adds a method to Warn a response from the AI Chat service
	/// </summary>
	/// <param name="logger">extension for ilogger</param>
	/// <param name="logString">string for the structured logger</param>
	/// <param name="structuredLogArgs">structured log parameters for the structured strings</param>
	public static void WarnAi(this ILogger logger, string logString, params object[] structuredLogArgs)
	{
		using (logger.BeginScope(
			       new Dictionary<string, object>()
			       {
				       ["LogType"] = "AiLog"
			       }
		       ))
		{
			logger.LogWarning(logString, structuredLogArgs);
		}
	}
}
