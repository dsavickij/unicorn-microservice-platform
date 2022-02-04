using MassTransit;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Queue;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public static class MessageBrokerConfigurationExtensions
{
    public static void AddMessageBroker(this IServiceCollection services, Action<MessageBrokerConfiguration> configure)
    {
        var cfg = new MessageBrokerConfiguration();
        configure(cfg);

        switch (cfg.Type)
        {
            case MessageBrokerType.Undefined:
                throw new ArgumentException("Message broker type is not provided");
            case MessageBrokerType.AzureServiceBus:
                AddAzureServiceBusMessageBroker(services, cfg);
                break;
            case MessageBrokerType.RabbitMq:
                AddRabbitMqMessageBroker(services, cfg);
                break;
            default:
                break;
        }
    }

    private static void AddRabbitMqMessageBroker(IServiceCollection services, MessageBrokerConfiguration cfg)
    {
        // TODO: check if initiliazation of HostSettings for RabbitMq and AzureSB can be unified
        var settings = new HostSettings
        {
            ConnectionString = cfg.ConnectionString,
            ServiceUri = new Uri(cfg.ConnectionString),
            RetryLimit = 3
        };

        var handler = (IBusRegistrationConfigurator configurator) =>
        {
            configurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(cfg.ConnectionString);
                configurator.AddQueueListeners(
                    context, cfg.OneWayMethods.Select(x => QueueNameFormatter.GetNamespaceBasedName(x.InterfaceMethod!)));
                configurator.AddEventListeners(context, cfg.SubscriptionId);
            });
        };

        services.AddMessageBroker(cfg, handler);
    }

    private static void AddAzureServiceBusMessageBroker(IServiceCollection services, MessageBrokerConfiguration cfg)
    {
        // TODO: check if initiliazation of HostSettings for RabbitMq and AzureSB can be unified
        var settings = new HostSettings
        {
            ConnectionString = cfg.ConnectionString,
            ServiceUri = new Uri(cfg.ConnectionString.Split("Endpoint=")[1]),
            RetryLimit = 3
        };

        var handler = (IBusRegistrationConfigurator configurator) =>
        {
            configurator.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(cfg.ConnectionString);
                configurator.AddQueueListeners(
                    context, cfg.OneWayMethods.Select(x => QueueNameFormatter.GetNamespaceBasedName(x.InterfaceMethod!)));
                configurator.AddEventListeners(context, cfg.SubscriptionId);
            });
        };

        services.AddMessageBroker(cfg, handler);

    }

    private static void AddMessageBroker(
        this IServiceCollection services,
        MessageBrokerConfiguration cfg,
        Action<IBusRegistrationConfigurator> configuratorHandler)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<QueueMessageHandler>();
            busConfigurator.AddEventConsumers();

            configuratorHandler(busConfigurator);
        });

        services.AddMassTransitHostedService();
        services.AddTransient<IQueueMessageDispatcher, QueueMessageDispatcher>();

        services.AddSingleton<IControllerMethodProvider>(
            _ => new ControllerMethodProvider(cfg.OneWayMethods.Select(x => x.ControllerMethod!)));

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
