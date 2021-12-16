using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public class ClientHostSettings : BaseHostSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}