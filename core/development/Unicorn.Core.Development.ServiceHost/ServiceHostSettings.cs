using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Development.ServiceHost;

public record ServiceHostSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
