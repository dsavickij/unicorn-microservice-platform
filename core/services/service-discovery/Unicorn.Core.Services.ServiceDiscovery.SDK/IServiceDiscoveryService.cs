using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Core.Services.ServiceDiscovery.SDK;

[UnicornHttpServiceMarker]
public interface IServiceDiscoveryService
{
    [UnicornHttpGet("GetGrpcServiceConfiguration/{serviceName}")]
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceName);

    [UnicornHttpGet("GetHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceName);

    [UnicornHttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    Task<HttpServiceConfiguration> UpdateHttpServiceConfigurationAsync(string serviceName, HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpPost("CreateHttpServiceConfiguration")]
    Task<HttpServiceConfiguration> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    Task DeleteHttpServiceConfigurationAsync(string serviceName);
}
