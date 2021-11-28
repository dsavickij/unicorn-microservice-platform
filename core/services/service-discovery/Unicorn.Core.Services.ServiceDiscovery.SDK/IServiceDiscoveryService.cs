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
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceName);

    [UnicornHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceName);

    [UnicornHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> UpdateHttpServiceConfigurationAsync(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration2/{serviceName}")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfigurationAsync(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    Task DeleteHttpServiceConfigurationAsync(string serviceName);
}
