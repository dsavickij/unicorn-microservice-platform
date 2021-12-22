using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

public class ClientHostController : BaseController
{ 
    private readonly ILogger<ClientHostController> _logger;
    private readonly IMyGrpcServiceClient _myGrpcSvcClient;
    private readonly IServiceDiscoveryService _svcDiscoveryService;
    private readonly IDevelopmentHttpService _developmentServiceHost;
    private readonly IAuthenticationScope _scopeProvider;

    public ClientHostController(
        IServiceDiscoveryService serviceDiscoveryService, 
        IMyGrpcServiceClient myGrpcServiceClient,
        IDevelopmentHttpService developmentServiceHost,
        IAuthenticationScope scopeProvider,
        ILogger<ClientHostController> logger)
    {
        _myGrpcSvcClient = myGrpcServiceClient;
        _svcDiscoveryService = serviceDiscoveryService;
        _developmentServiceHost = developmentServiceHost;
        _scopeProvider = scopeProvider;
        _logger = logger;
    }

    [HttpGet("GetHttpServiceConfiguration")]
    public async Task<HttpServiceConfiguration> Get() => await SendAsync(new GetHttpServiceConfigurationRequest());

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
