using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

public record HttpServiceConfiguration
{
    public string ServiceHostName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
