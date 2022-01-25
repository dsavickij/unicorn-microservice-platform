namespace Unicorn.Core.Infrastructure.Communication.MessageBroker.Messages;

public record UnicornQueueMessage
{
    public string MethodName { get; set; } = string.Empty;
    public IEnumerable<Argument> Arguments { get; set; } = Array.Empty<Argument>();
}
