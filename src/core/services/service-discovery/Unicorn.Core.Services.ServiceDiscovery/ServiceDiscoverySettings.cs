using Unicorn.Core.Infrastructure.Host.SDK.Settings;
using Unicorn.Core.Services.ServiceDiscovery.SDK;

namespace Unicorn.Core.Services.ServiceDiscovery;

public record ServiceDiscoverySettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
