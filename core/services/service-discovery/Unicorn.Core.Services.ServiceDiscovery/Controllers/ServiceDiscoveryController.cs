using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Services.ServiceDiscovery.Features.GetGrpcServiceConfiguration;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Controllers;

public class ServiceDiscoveryController : UnicornBaseController<IServiceDiscoveryService>, IServiceDiscoveryService
{
    private readonly ILogger<ServiceDiscoveryController> _logger;

    public ServiceDiscoveryController(ILogger<ServiceDiscoveryController> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/configurations/{serviceHostName}/grpcServiceConfiguration")]
    public async Task<OperationResult<GrpcServiceConfiguration>> GetGrpcServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogInformation($"Executing GetGrpcServiceConfiguration for {serviceHostName}");

        return await SendAsync(new GetGrpcServiceConfigurationRequest { ServiceHostName = serviceHostName });
    }

    [HttpGet("api/configurations/{serviceHostName}/httpServiceConfiguration")]
    public async Task<OperationResult<HttpServiceConfiguration>> GetHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogDebug($"Executing GetHttpServiceConfiguration/{serviceHostName}");

        return await SendAsync(new GetHttpServiceConfigurationRequest { ServiceHostName = serviceHostName });
    }

    [HttpPut("api/configurations/{serviceHostName}/httpServiceConfiguration")]
    public Task<OperationResult<HttpServiceConfiguration>> UpdateHttpServiceConfigurationAsync(string serviceHostName, HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"UpdateHttpServiceConfiguration");

        return Task.FromResult(new OperationResult<HttpServiceConfiguration>(OperationStatusCode.Status200OK, httpServiceConfiguration));
    }

    [HttpPost("api/configurations/{serviceHostName}/httpServiceConfiguration")]
    public Task<OperationResult<HttpServiceConfiguration>> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"CreateHttpServiceConfiguration");

        return Task.FromResult(new OperationResult<HttpServiceConfiguration>(OperationStatusCode.Status200OK, httpServiceConfiguration));
    }

    [HttpDelete("api/configurations/{serviceHostName}")]
    public Task DeleteHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger.LogInformation($"DeleteHttpServiceConfiguration");

        return Task.CompletedTask;
    }
}
