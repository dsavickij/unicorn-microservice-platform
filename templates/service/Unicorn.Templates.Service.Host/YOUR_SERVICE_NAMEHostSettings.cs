using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

/// <summary>
/// The properties of this class are binded from appSettings. 
/// Changes in this class should be refleced in appSettings for configuration to be binded properly
/// </summary>
public record YOUR_SERVICE_NAMEHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}