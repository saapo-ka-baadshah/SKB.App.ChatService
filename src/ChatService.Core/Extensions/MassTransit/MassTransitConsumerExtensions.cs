using MassTransit;
using SKB.App.ChatService.EventConsumers;

namespace SKB.App.ChatService.Core.Extensions.MassTransit;

/// <summary>
/// Adds mass transit consumer extensions
/// </summary>
public static class MassTransitConsumerExtensions
{
	/// <summary>
	/// Adds chat consumers to the masstransit bus
	/// </summary>
	/// <param name="configurator">Mass transits bus configurator <see cref="IBusRegistrationConfigurator"/></param>
	/// <returns>Extended bus configurator</returns>
	public static IBusRegistrationConfigurator AddChatConsumer(
		this IBusRegistrationConfigurator configurator
	)
	{
		configurator.AddConsumers(
				assemblies: [
					typeof(IEventConsumersAssemblyMarker).Assembly
				]
			);
		return configurator;
	}
}
