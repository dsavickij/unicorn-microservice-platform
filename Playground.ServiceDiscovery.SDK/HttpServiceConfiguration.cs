using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.ServiceDiscovery.SDK;

public record HttpServiceConfiguration
{
    public string Name { get; set; } = "";
    public string BaseUrl { get; set; } = "";
}
