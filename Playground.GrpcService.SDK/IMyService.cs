using Playground.Common.SDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService.SDK;

[PlaygroundHttpServiceMarker]
public interface IMyService
{
    Task<string> Get();
}
