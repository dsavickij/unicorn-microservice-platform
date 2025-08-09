using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.Host.SDK.Settings;

public abstract record BaseHostSettings
{
    public abstract string ServiceHostName { get; }
    public AuthenticationSettings AuthenticationSettings { get; set; } = new();
    public OneWayCommunicationSettings OneWayCommunicationSettings { get; set; } = new();
    public ServiceDiscoverySettings ServiceDiscoverySettings { get; set; } = new();
}

internal static class InternalBaseHostSettings
{
    public static string ServiceHostName { get; set; }

    public static AuthenticationSettings AuthenticationSettings { get; set; } = new();

    public static OneWayCommunicationSettings OneWayCommunicationSettings { get; set; } = new();

    public static ServiceDiscoverySettings ServiceDiscoverySettings { get; set; } = new();
}