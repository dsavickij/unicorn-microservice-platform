using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Controllers;

[ApiController]
public class ServiceDiscoveryController : ControllerBase, IServiceDiscoveryService
{
    private readonly ILogger<ServiceDiscoveryController> _logger;

    public ServiceDiscoveryController(ILogger<ServiceDiscoveryController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetGrpcServiceConfiguration/{serviceName}")]
    public Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceName)
    {
        _logger.LogInformation($"Executing GetGrpcServiceConfiguration for {serviceName}");

        const int port = 5080;

        return Task.FromResult(new GrpcServiceConfiguration
        {
            Name = serviceName,
            BaseUrl = "http://localhost:5080",
            Port = port
        });
    }

    [HttpGet("GetHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceName)
    {
        _logger.LogDebug($"Executing GetHttpServiceConfiguration/{serviceName}");

        var services = new[]
        {
           new HttpServiceConfiguration
           {
                Name = Constants.ServiceName,
                BaseUrl = "http://localhost:5081"
           }
        };

        var svc = services.FirstOrDefault(x => x.Name == serviceName) ?? new HttpServiceConfiguration
        {
            Name = "Non existing service",
            BaseUrl = "http://localhost:5080"
        };

        return Task.FromResult(svc);
    }

    [HttpPut("UpdateHttpServiceConfiguration/{serviceName}")]
    public Task<HttpServiceConfiguration> UpdateHttpServiceConfigurationAsync(string serviceName, HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"UpdateHttpServiceConfiguration");

        return Task.FromResult(httpServiceConfiguration);
    }

    [HttpPost("CreateHttpServiceConfiguration")]
    public Task<HttpServiceConfiguration> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"CreateHttpServiceConfiguration");

        return Task.FromResult(httpServiceConfiguration);
    }

    [HttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    public Task DeleteHttpServiceConfigurationAsync(string serviceName)
    {
        _logger.LogInformation($"DeleteHttpServiceConfiguration");

        return Task.CompletedTask;
    }
}
