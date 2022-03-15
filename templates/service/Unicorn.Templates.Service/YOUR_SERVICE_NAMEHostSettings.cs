using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;
using Unicorn.Templates.Service.SDK;

namespace Unicorn.Templates.Service;

// Property values of this record are binded from appSettings.
// Changes in this class should be reflected in appSettings for configuration to be binded properly

public record YOUR_SERVICE_NAMEHostSettings : BaseHostSettings
{
    public override string ServiceHostName => Constants.ServiceHostName;
    public string DbConnectionString { get; set; } = string.Empty;
}
