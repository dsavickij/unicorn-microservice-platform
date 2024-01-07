namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public record OneWayCommunicationSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; }
}
