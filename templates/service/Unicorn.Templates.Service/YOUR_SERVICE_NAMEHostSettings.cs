using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

// Property values of this record are binded from appSettings. 
// Changes in this class should be reflected in appSettings for configuration to be binded properly

public record YOUR_SERVICE_NAMEHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}