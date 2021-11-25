using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;
using Playground.Core.Services.ServiceDiscovery.SDK;
using Playground.Core.Services.ServiceDiscovery.SDK.Configurations;

[assembly: PlaygroundAssemblyServiceName(Constants.ServiceName)]

namespace Playground.Core.Services.ServiceDiscovery.SDK;

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
