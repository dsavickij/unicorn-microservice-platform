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
    public Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName);

    //---------------------------------

    [PlaygroundHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName);

    [PlaygroundHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> UpdateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [PlaygroundHttpPost("CreateHttpServiceConfiguration")]
    public Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(HttpServiceConfiguration httpServiceConfiguration);

    [PlaygroundHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    public Task DeleteHttpServiceConfiguration(string serviceName);
}
