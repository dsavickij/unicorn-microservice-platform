using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Core.Services.ServiceDiscovery.SDK.Configurations;

public record GrpcServiceConfiguration
{
    public string Name { get; set; } = "";
    public string BaseUrl { get; set; } = "";
    public int Port { get; set; }
}
