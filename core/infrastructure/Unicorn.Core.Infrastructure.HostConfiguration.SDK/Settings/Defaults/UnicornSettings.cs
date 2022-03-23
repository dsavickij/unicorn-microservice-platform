using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;

public static class UnicornSettings
{
    public static class HealthCheck
    {
        public static string Pattern => "/hc";

        public static HealthCheckOptions Options => new()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };
    }
}
