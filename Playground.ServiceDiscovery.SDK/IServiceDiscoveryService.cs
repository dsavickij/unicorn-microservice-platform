using Playground.Common.SDK.Abstractions;
using Playground.Common.SDK.Abstractions.Http;
using Playground.Common.SDK.Abstractions.Http.MethodAttributes;
using Playground.ServiceDiscovery.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: PlaygroundAssemblyServiceName(Constants.ServiceName)]

namespace Playground.ServiceDiscovery.SDK;

[PlaygroundHttpServiceMarker]
public interface IServiceDiscoveryService
{
    [PlaygroundHttpGet("GetGrpcServiceConfiguration")]
    Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName);

    [PlaygroundHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName);

    [PlaygroundHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> UpdateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [PlaygroundHttpPost("CreateHttpServiceConfiguration")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(HttpServiceConfiguration httpServiceConfiguration);

    [PlaygroundHttpPost("CreateHttpServiceConfiguration2/{serviceName}")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [PlaygroundHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    Task DeleteHttpServiceConfiguration(string serviceName);
}
