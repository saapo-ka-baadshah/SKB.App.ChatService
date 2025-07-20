using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKB.App.ChatService.Abstractions.Options;

namespace SKB.App.ChatService.Core.Extensions;

/// <summary>
/// Provides extensions for the prompt options
/// </summary>
public static class PromptExtensions
{
	/// <summary>
	/// Adds prompt options to the services configurations
	/// </summary>
	/// <param name="services">Extension candidate builder service collection.</param>
	/// <param name="configuration">Builder configuration.</param>
	/// <returns>Extended service collection.</returns>
	public static IServiceCollection AddPromptOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<PromptOptions>(
				options =>
				{
					List<string>? defaultSystemPrompts =
						configuration
							.GetSection($"{nameof(PromptOptions)}:SystemChatPromptList")
							.Get<List<string>>();

					List<string>? defaultUserPrompts =
						configuration
							.GetSection($"{nameof(PromptOptions)}:DefaultUserChatPromptList")
							.Get<List<string>>();

					List<string>? defaultMcpToolPrompts =
						configuration
							.GetSection($"{nameof(PromptOptions)}:McpToolInstructionPrompt")
							.Get<List<string>>();

					options.SystemChatPromptList =
						defaultSystemPrompts
						?? new PromptOptions().SystemChatPromptList;

					options.DefaultUserChatPromptList =
						defaultUserPrompts
						?? new PromptOptions().DefaultUserChatPromptList;

					options.McpToolInstructionPrompt =
						defaultMcpToolPrompts
						?? new PromptOptions().McpToolInstructionPrompt;
				});

		return services;
	}
}
