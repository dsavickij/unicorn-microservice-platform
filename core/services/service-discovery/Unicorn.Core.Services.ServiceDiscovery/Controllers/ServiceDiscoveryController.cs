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
    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceName)
    {
        _logger.LogInformation($"Executing GetGrpcServiceConfiguration for {serviceName}");

        var services = new[]
        {
            new GrpcServiceConfiguration
            {
                Name = "GreeterProtoService",
                BaseUrl = "https://localhost:5080",
                Port = 5080
            },
            new GrpcServiceConfiguration
            {
                Name = "MyGrpcService",
                BaseUrl = "http://localhost:5287",
                Port = 7287
            },
            new GrpcServiceConfiguration
            {
                 Name = "DiscountGrpcService",

                 // BaseUrl = "http://localhost:5220",
                 BaseUrl = "https://unicorn.eshop.discount:443",
                 Port = 80
            }
        };

        return services.FirstOrDefault(x => x.Name == serviceName, new GrpcServiceConfiguration());
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
           },
           new HttpServiceConfiguration
           {
                Name = "Development.HttpService",
                BaseUrl = "https://localhost:7287"
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
