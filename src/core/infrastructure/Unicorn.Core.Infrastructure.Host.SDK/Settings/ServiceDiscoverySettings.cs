namespace Unicorn.Core.Infrastructure.Host.SDK.Settings;

public record ServiceDiscoverySettings
{
    public string Url { get; set; } = string.Empty;

    public bool ExecuteSelfRegistration { get; set; } = true;
}
