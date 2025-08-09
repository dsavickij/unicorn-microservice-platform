using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateGrpcServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateHttpServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetAllHttpServiceConfigurations;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetGrpcServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetHttpServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateGrpcServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateHttpServiceConfiguration;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest;

[AllowAnonymous]
public class ServiceDiscoveryService : UnicornHttpService<IServiceDiscoveryService>, IServiceDiscoveryService
{
    private readonly ILogger<ServiceDiscoveryService> _logger;

    public ServiceDiscoveryService(ILogger<ServiceDiscoveryService> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/configurations/{serviceHostName}/grpc")]
    public async Task<OperationResult<GrpcServiceConfiguration>> GetGrpcServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogInformation($"Executing GetGrpcServiceConfiguration for {serviceHostName}");

        return await SendAsync(new GetGrpcServiceConfigurationRequest { ServiceHostName = serviceHostName });
    }

    [HttpGet("api/configurations/{serviceHostName}/http")]
    public async Task<OperationResult<HttpServiceConfiguration>> GetHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogDebug($"Executing GetHttpServiceConfiguration/{serviceHostName}");

        return await SendAsync(new GetHttpServiceConfigurationRequest { ServiceHostName = serviceHostName });
    }

    [HttpGet("api/configurations/http/all")]
    public async Task<OperationResult<IEnumerable<HttpServiceConfiguration>>> GetAllHttpServiceConfigurationsAsync()
    {
        return await SendAsync(new GetAllHttpServiceConfigurationsRequest());
    }

    [HttpPut("api/configurations/http")]
    public Task<OperationResult<HttpServiceConfiguration>> UpdateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        return SendAsync(new UpdateHttpServiceConfigurationRequest { Configuration = httpServiceConfiguration });
    }

    [HttpPut("api/configurations/grpc")]
    public Task<OperationResult<GrpcServiceConfiguration>> UpdateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration)
    {
        return SendAsync(new UpdateGrpcServiceConfigurationRequest { Configuration = grpcServiceConfiguration });
    }

    [HttpPost("api/configurations/http")]
    public Task<OperationResult> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        return SendAsync(new CreateHttpServiceConfigurationRequest { Configuration = httpServiceConfiguration });
    }

    [HttpPost("api/configurations/grpc")]
    public Task<OperationResult> CreateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration)
    {
        return SendAsync(new CreateGrpcServiceConfigurationRequest { Configuration = grpcServiceConfiguration });
    }

    [HttpDelete("api/configurations/{serviceHostName}")]
    public Task DeleteHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogInformation($"DeleteHttpServiceConfiguration");

        return Task.CompletedTask;
    }
}
