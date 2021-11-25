using Microsoft.AspNetCore.Mvc;
using Unicorn.GrpcService.SDK.Grpc.Clients;

namespace Unicorn.GrpcServiceClient.Controllers;

[ApiController]
[Route("[controller]")]
public class GrpcServiceClientController : ControllerBase
{
    //private readonly IMyService _svcDiscovery;
    private readonly IGreeterProtoClient _greeterProtoClient;
    private readonly ILogger<GrpcServiceClientController> _logger;

    public GrpcServiceClientController(
        //IMyService serviceDiscoveryService,
        //IGreeterProtoClient greeterProtoClient, 
        ILogger<GrpcServiceClientController> logger)
    {
        //_svcDiscovery = serviceDiscoveryService;
        //_greeterProtoClient = greeterProtoClient;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get()
    {
        //var client = _provider.GetRequiredService<IGreeterProtoClient>();

        //var r = await _svcDiscovery.Get();

        var result = await _greeterProtoClient.SayHelloAsync(new GrpcService1.HelloRequest { Name = "Dmitrij S" });

        return result.Message;

        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //{
        //    Date = DateTime.Now.AddDays(index),
        //    TemperatureC = Random.Shared.Next(-20, 55),
        //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //})
        //.ToArray();
    }
}
