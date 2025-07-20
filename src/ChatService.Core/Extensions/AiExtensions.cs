using System.Net;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using OllamaSharp;
using SKB.App.ChatService.Core.Options;
using SKB.Core.Policies.Reactive.RetryFamily;

namespace SKB.App.ChatService.Core.Extensions;

/// <summary>
/// Adds all the AI Extensions
/// </summary>
public static class AiExtensions
{
	/// <summary>
	/// Adds the AI services to the builder service collection
	/// </summary>
	/// <param name="services">Service collection for extensions subjection</param>
	/// <param name="configuration">Configuration from the builder</param>
	/// <returns>Extended service collection <see cref="IServiceCollection"/></returns>
	public static IServiceCollection AddAiServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		ILogger logger = services.BuildServiceProvider()
			.GetRequiredService<ILoggerFactory>()
			.CreateLogger("PreMaterialization_phase:AiExtensions");
		OllamaOptions? ollamaOptions = configuration.GetSection(OllamaOptions.Key).Get<OllamaOptions>();

		if (ollamaOptions == null)
		{
			logger.LogWarning("Ollama options not found! Skipping Ollama chat integration…");
			return services;
		}

		// Inject the IChatClient
		services.AddSingleton<IChatClient>(_ =>
		{

			IChatClient ollamaClient = new OllamaApiClient(
				ollamaOptions.GetHostUri(),
				ollamaOptions.Model
			);
			// TODO: add a policy to retry after 3 tries after a reasonable delay
			var builder = ollamaClient.AsBuilder()
				.UseFunctionInvocation();

			var chatClient = RetryPolicy
				.ForeverRetryPolicy<HttpListenerException>()
				.Execute(() => builder.Build());

			return chatClient;
		});

		// TODO: Add a policy to retry 3 times after a reasonable delay
		SseClientTransportOptions? clientTransportOptions = configuration
			.GetSection(nameof(SseClientTransportOptions))
			.Get<SseClientTransportOptions>();
		if (clientTransportOptions == null)
		{
			logger
				.LogCritical(
					"AI Services need a running MCP server! Options for the MCP server transport not provided.");
			throw new Exception("SseClientTransportOptions is null");
		}

		try
		{
			var mcpClient = RetryPolicy
				.CountLimitedRetryPolicy<HttpRequestException>()
				.Execute(
					() =>
					{
						return McpClientFactory.CreateAsync(
							new SseClientTransport(clientTransportOptions)
						).GetAwaiter().GetResult();
					});

			services.AddSingleton<IMcpClient>(_ => mcpClient);
			try
			{
				services.AddSingleton(mcpClient.ListToolsAsync().GetAwaiter().GetResult());
			}
			catch (Exception e)
			{
				logger.LogError("Failure in loading MCP tools list. {error}", e);
				services.AddSingleton<IList<McpClientTool>>(_ => new List<McpClientTool>());
			}
		}
		catch (Exception e)
		{
			logger.LogError("Failed to connect to the MCP SSE server. ({ErrorMessage})", e.Message);
		}


		return services;
	}
}
