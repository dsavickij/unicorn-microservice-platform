using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Core.Services.ServiceDiscovery.SDK;

[UnicornHttpServiceMarker]
public interface IServiceDiscoveryService
{
    [UnicornHttpGet("api/configurations/{serviceHostName}/grpc")]
    Task<OperationResult<GrpcServiceConfiguration>> GetGrpcServiceConfigurationAsync(string serviceHostName);

    [UnicornHttpGet("api/configurations/{serviceHostName}/http")]
    Task<OperationResult<HttpServiceConfiguration>> GetHttpServiceConfigurationAsync(string serviceHostName);

    [UnicornHttpPut("api/configurations/http")]
    Task<OperationResult<HttpServiceConfiguration>> UpdateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    [UnicornHttpGet("api/configurations/http/all")]
    Task<OperationResult<IEnumerable<HttpServiceConfiguration>>> GetAllHttpServiceConfigurationsAsync();

    [UnicornHttpPut("api/configurations/grpc")]
    Task<OperationResult<GrpcServiceConfiguration>> UpdateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration);

    // TODO: create configuration method should probably be removed from SDK
    [UnicornHttpPost("api/configurations/{serviceHostName}/http")]
    Task<OperationResult> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    // TODO: delete configuration method should probably be removed from SDK
    [UnicornHttpDelete("api/configurations/{serviceHostName}")]
    Task DeleteHttpServiceConfigurationAsync(string serviceHostName);
}
