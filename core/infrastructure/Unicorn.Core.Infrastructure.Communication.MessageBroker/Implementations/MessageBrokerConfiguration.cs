using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker.Implementations;

public record MessageBrokerConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public IEnumerable<string> ReceiveQueueNames { get; set; } = Array.Empty<string>();
    public IEnumerable<MethodInfo> ReceiveMethods { get; set; } = Array.Empty<MethodInfo>();
}
