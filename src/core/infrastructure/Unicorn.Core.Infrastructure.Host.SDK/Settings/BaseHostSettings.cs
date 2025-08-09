using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.Host.SDK.Settings;

public abstract record BaseHostSettings
{
    public abstract string ServiceHostName { get; }
    public AuthenticationSettings AuthenticationSettings { get; set; } = new AuthenticationSettings();
    public OneWayCommunicationSettings OneWayCommunicationSettings { get; set; } = new OneWayCommunicationSettings();
    public ServiceDiscoverySettings ServiceDiscoverySettings { get; set; } = new ServiceDiscoverySettings();
}

internal static class InternalBaseHostSettings
{
    public static string ServiceHostName { get; set; }

    public static AuthenticationSettings AuthenticationSettings { get; set; } = new AuthenticationSettings();
    public static OneWayCommunicationSettings OneWayCommunicationSettings { get; set; } = new OneWayCommunicationSettings();
    public static ServiceDiscoverySettings ServiceDiscoverySettings { get; set; } = new ServiceDiscoverySettings();
}
