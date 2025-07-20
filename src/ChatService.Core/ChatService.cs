using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKB.App.ChatService.Core.Extensions;

namespace SKB.App.ChatService.Core;

/// <summary>
/// Add chat services to the application
/// </summary>
public static class ChatService
{
	/// <summary>
	/// Adds all core chat services to the application
	/// </summary>
	/// <param name="services">Extension candidate service collection.</param>
	/// <param name="configuration">Builder configuration.</param>
	/// <returns>Extended service collection</returns>
	public static IServiceCollection AddCoreChatService(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddPromptOptions(configuration);
		services.AddAiServices(configuration);
		return services;
	}
}
