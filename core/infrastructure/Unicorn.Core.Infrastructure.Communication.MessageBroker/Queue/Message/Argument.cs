namespace Unicorn.Core.Infrastructure.Communication.MessageBroker.Queue.Message;

public record Argument
{
    public string TypeName { get; set; } = string.Empty;
    public object Value { get; set; } = new object();
}
