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
    public Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName)
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
    public Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
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
    public Task<HttpServiceConfiguration> UpdateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"UpdateHttpServiceConfiguration");

        return Task.FromResult(httpServiceConfiguration);
    }

    [HttpPost("CreateHttpServiceConfiguration")]
    public Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"CreateHttpServiceConfiguration");

        return Task.FromResult(httpServiceConfiguration);
    }

    [HttpDelete("DeleteHttpServiceConfiguration/{serviceName}")]
    public Task DeleteHttpServiceConfiguration(string serviceName)
    {
        _logger.LogInformation($"DeleteHttpServiceConfiguration");

        return Task.CompletedTask;
    }

    [HttpPost("CreateHttpServiceConfiguration2/{serviceName}")]
    public Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration)
    {
        _logger.LogInformation($"CreateHttpServiceConfiguration2");

        return Task.FromResult(httpServiceConfiguration);
    }
}
