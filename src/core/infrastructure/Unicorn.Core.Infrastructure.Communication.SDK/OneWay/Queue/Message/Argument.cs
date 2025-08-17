namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue.Message;

public record Argument
{
    public string TypeName { get; set; } = string.Empty;
    public object Value { get; set; } = new object();
}
