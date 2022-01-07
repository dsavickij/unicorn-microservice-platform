using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public record ServiceHostSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}