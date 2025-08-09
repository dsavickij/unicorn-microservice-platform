using Refit;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.Core.Services.ServiceDiscovery.SDK;

[UnicornRestServiceMarker]
public interface IServiceDiscoveryService
{
    [Get("api/configurations/{serviceHostName}/grpc")]
    Task<OperationResult<GrpcServiceConfiguration>> GetGrpcServiceConfigurationAsync(string serviceHostName);

    [Get("api/configurations/{serviceHostName}/http")]
    Task<OperationResult<HttpServiceConfiguration>> GetHttpServiceConfigurationAsync(string serviceHostName);

    [Put("api/configurations/http")]
    Task<OperationResult<HttpServiceConfiguration>> UpdateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    [Get("api/configurations/http/all")]
    Task<OperationResult<IEnumerable<HttpServiceConfiguration>>> GetAllHttpServiceConfigurationsAsync();

    [Put("api/configurations/grpc")]
    Task<OperationResult<GrpcServiceConfiguration>> UpdateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration);

    // TODO: create configuration method should probably be removed from SDK
    [Post("api/configurations/{serviceHostName}/http")]
    Task<OperationResult> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    // TODO: delete configuration method should probably be removed from SDK
    [Delete("api/configurations/{serviceHostName}")]
    Task DeleteHttpServiceConfigurationAsync(string serviceHostName);
}
