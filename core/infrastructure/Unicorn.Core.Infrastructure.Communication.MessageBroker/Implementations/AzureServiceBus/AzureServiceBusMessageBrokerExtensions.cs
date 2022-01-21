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

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UnicornOneWayMessageConsumer>();
            x.UsingAzureServiceBus((context, configurator) =>
            {
                configurator.Host(cfg.ConnectionString);
                configurator.AddReceiveEndpoints(context, cfg.ReceiveQueueNames);
            });
        });

        services.AddMassTransitHostedService();
        services.AddTransient<IOneWayMethodInvocationExecutor, OneWayMethodInvocationExecutor>();
        services.AddTransient<IOneWayControllerMethodProvider>(_ => new OneWayControllerMethodProvider(cfg.ReceiveMethods));
    }

    private static void AddReceiveEndpoints(this IReceiveConfigurator configurator, IBusRegistrationContext context, IEnumerable<string> receiveQueueNames)
    {
        //TODO: add validation on UnicornHttpService for one way method name uniqueness
        foreach (var queueName in receiveQueueNames)
        {
            configurator.ReceiveEndpoint(queueName, c => c.ConfigureConsumer<UnicornOneWayMessageConsumer>(context));
        }
    }
}
