using MassTransit;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker.Implementations.AzureServiceBus;

public static class AzureServiceBusMessageBrokerExtensions
{
    public static void AddAzureServiceBusMessageBroker(this IServiceCollection services, Action<MessageBrokerConfiguration> configure)
    {
        var cfg = new MessageBrokerConfiguration();
        configure(cfg);

        var settings = new HostSettings
        {
            ConnectionString = cfg.ConnectionString,
            ServiceUri = new Uri(cfg.ConnectionString.Split("Endpoint=")[1]),
            RetryLimit = 3
        };

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<QueueMessageHandler>();
            busConfigurator.AddEventConsumers();

            busConfigurator.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(cfg.ConnectionString);
                configurator.AddQueueListeners(context, cfg.OneWayMethods.Select(x => QueueNameFormatter.GetNamespaceBasedName(x.InterfaceMethod)));
                configurator.AddEventListeners(context, cfg.SubscriptionId);
            });
        });

        services.AddMassTransitHostedService();
        services.AddTransient<IQueueMessageDispatcher, QueueMessageDispatcher>();

        services.AddSingleton<IControllerMethodProvider>(
            _ => new ControllerMethodProvider(cfg.OneWayMethods.Select(x => x.ControllerMethod)));

        services.AddTransient<IUnicornEventPublisher, UnicornEventPublisher>();
    }

    private static void AddQueueListeners(
        this IReceiveConfigurator configurator, 
        IBusRegistrationContext context, 
        IEnumerable<string> receiveQueueNames)
    {
        //TODO: add validation on UnicornHttpService for one way method name uniqueness
        foreach (var queueName in receiveQueueNames)
        {
            configurator.ReceiveEndpoint(queueName, c => c.ConfigureConsumer<QueueMessageHandler>(context));
        }
    }

    private static void AddEventListeners(
        this IReceiveConfigurator configurator, 
        IBusRegistrationContext context, 
        Guid subscriptionId)
    {
        foreach (var handler in AssemblyScanner.GetEventHandlers())
        {
            configurator.ReceiveEndpoint(subscriptionId.ToString(), c => c.ConfigureConsumer(context, handler));
        }
    }

    private static void AddEventConsumers(this IRegistrationConfigurator configurator)
    {
        foreach (var handler in AssemblyScanner.GetEventHandlers())
        {
            configurator.AddConsumer(handler);
        }
    }
}
