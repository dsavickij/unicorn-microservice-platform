using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;
using Unicorn.Core.Infrastructure.Development.ServiceHost.SDK;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMyGrpcServiceClient _myGrpcSvcClient;
    private readonly IServiceDiscoveryService _svcDiscoveryService;
    private readonly IDevelopmentHttpService _developmentServiceHost;
    private readonly IAuthenticationScope _scopeProvider;

    public WeatherForecastController(
        IServiceDiscoveryService serviceDiscoveryService, 
        IMyGrpcServiceClient myGrpcServiceClient,
        IDevelopmentHttpService developmentServiceHost,
        IAuthenticationScope scopeProvider,
        ILogger<WeatherForecastController> logger)
    {
        _myGrpcSvcClient = myGrpcServiceClient;
        _svcDiscoveryService = serviceDiscoveryService;
        _developmentServiceHost = developmentServiceHost;
        _scopeProvider = scopeProvider;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<HttpServiceConfiguration> Get()
    {
        _logger.LogTrace($"Executing GetWeatherForecast at {DateTime.UtcNow}");
        _logger.LogDebug("Debug msg");
        _logger.LogInformation("Info message");
        _logger.LogWarning("Warning message");
        _logger.LogError("Error at 123");

        using var scope = _scopeProvider.EnterServiceUserScope();

        var r = await _myGrpcSvcClient.Multiply(5, 6);

        //var name2 = await _developmentServiceHost.GetNameAsync("dsdsds");

        //var name = await _developmentServiceHost.GetNameAsync();

        return new HttpServiceConfiguration { Name = r.ToString() };

    }

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task<HttpServiceConfiguration> GetName(string name)
    {
        return await _svcDiscoveryService.GetHttpServiceConfigurationAsync("ddd");
    }

    [HttpPut("UploadFile")]
    public async Task UploadFileAsync(IFormFile file)
    {
        var r1 = await _developmentServiceHost.UploadFileAsync2("dsds", "ssss");

        var r2 = await _developmentServiceHost.UploadFileAsync("dd", "s", file);

        return;
    }
}
