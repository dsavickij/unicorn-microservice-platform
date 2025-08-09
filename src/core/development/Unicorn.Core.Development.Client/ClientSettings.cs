using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Development.Client;

public record ClientSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public override string ServiceHostName => "Unicorn.Core.Development.Client";
}