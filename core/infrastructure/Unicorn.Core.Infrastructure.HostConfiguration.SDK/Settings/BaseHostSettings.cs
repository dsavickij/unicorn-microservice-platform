using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public record BaseHostSettings
{
    public AuthenticationSettings AuthenticationSettings { get; set; } = new AuthenticationSettings();
    public OneWayCommunicationSettings OneWayCommunicationSettings { get; set; } = new OneWayCommunicationSettings();
    public ServiceDiscoverySettings ServiceDiscoverySettings { get; set; } = new ServiceDiscoverySettings();
}
