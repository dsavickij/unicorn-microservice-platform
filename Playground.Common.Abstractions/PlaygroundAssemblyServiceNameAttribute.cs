using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.Abstractions;

//
// Summary:
//     Attribute to specify service host ServiceName on public SDK assembly's level
[AttributeUsage(AttributeTargets.Assembly)]
public class PlaygroundAssemblyServiceNameAttribute : Attribute
{
    //
    // Summary:
    //     The value to match with service host ServiceName on public SDK assembly's level,
    //     may be overriden by ETPServiceAttribute ServiceName value.
    public string ServiceName { get; }

    public PlaygroundAssemblyServiceNameAttribute(string serviceName)
    {
        ServiceName = serviceName;
    }
}
