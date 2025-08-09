using Unicorn.Core.Infrastructure.Host.SDK.Settings;

public record ClientHostSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public override string ServiceHostName => "Unicorn.Core.Development.ClientHost";
}
