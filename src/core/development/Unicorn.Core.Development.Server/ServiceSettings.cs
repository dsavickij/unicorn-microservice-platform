using Unicorn.Core.Development.Server.SDK;
using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Development.Server;

public sealed record ServiceSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
