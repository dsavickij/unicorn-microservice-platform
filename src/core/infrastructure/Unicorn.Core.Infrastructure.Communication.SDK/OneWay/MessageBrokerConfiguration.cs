using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay;

public record MessageBrokerConfiguration
{
    public MessageBrokerType Type { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; }
    public IEnumerable<OneWayMethodConfiguration> OneWayMethods { get; set; } = Array.Empty<OneWayMethodConfiguration>();
}

public record OneWayMethodConfiguration
{
    /// <summary>
    /// Method in HTTP service interface used to create queue name and start listening to it
    /// </summary>
    public MethodInfo? InterfaceMethod { get; set; }

    /// <summary>
    /// Implementation of method in HTTP service interface used to call after receving a message from queue
    /// </summary>
    public MethodInfo? ControllerMethod { get; set; }
}

public enum MessageBrokerType
{
    Undefined = 0,
    AzureServiceBus,
    RabbitMq
}
