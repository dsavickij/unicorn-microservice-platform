using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Services.ServiceDiscovery;

public record ServiceDiscoveryHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
