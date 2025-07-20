using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SKB.App.ChatService.Core.Extensions;

namespace SKB.App.ChatService.Core;

/// <summary>
/// Public Start-point for the program
/// </summary>
[PublicAPI]
internal class Program
{
    /// <summary>
    /// Main method to call the SKB.ChatService Console program
    /// </summary>
    /// <param name="args">arguments pass through</param>
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Add Logging
        builder.Services.AddLogging(
				configure =>
				{
					configure.AddConsole();
				});

        // Add Configurations
        builder.AddConfigurations();

        // Add all Chat services to the program
        builder.Services.AddCoreChatService(builder.Configuration);

        //builder.Services.AddHostedService<TestControlService>();

        var app = builder.Build();
        app.Run();
    }
}
