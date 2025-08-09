using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Development.Service;

public record ServiceSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
