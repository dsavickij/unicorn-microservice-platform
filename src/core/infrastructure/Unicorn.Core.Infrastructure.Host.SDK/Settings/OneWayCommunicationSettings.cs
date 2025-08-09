namespace Unicorn.Core.Infrastructure.Host.SDK.Settings;

public record OneWayCommunicationSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; }
}
