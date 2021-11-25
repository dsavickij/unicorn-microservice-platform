using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

[assembly: UnicornAssemblyServiceName(Constants.ServiceName)]

namespace Unicorn.Core.Services.ServiceDiscovery.SDK;

[UnicornHttpServiceMarker]
public interface IServiceDiscoveryService
{
    [UnicornHttpGet("GetGrpcServiceConfiguration")]
    Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName);

    [UnicornHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName);

    [UnicornHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> UpdateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration2/{serviceName}")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    Task DeleteHttpServiceConfiguration(string serviceName);
}
