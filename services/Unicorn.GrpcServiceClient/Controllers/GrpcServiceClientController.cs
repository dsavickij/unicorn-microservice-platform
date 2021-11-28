using Microsoft.AspNetCore.Mvc;
using Unicorn.GrpcService.SDK.Grpc.Clients;

namespace Unicorn.GrpcServiceClient.Controllers;

[ApiController]
[Route("[controller]")]
public class GrpcServiceClientController : ControllerBase
{
    private readonly IGreeterProtoClient _greeterProtoClient;

    private readonly ILogger<GrpcServiceClientController> _logger;

    public GrpcServiceClientController(
        ILogger<GrpcServiceClientController> logger,
        IGreeterProtoClient greeterProtoClient)
    {
        _logger = logger;
        _greeterProtoClient = greeterProtoClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get()
    {
        var result = await _greeterProtoClient.SayHelloAsync(new GrpcService1.HelloRequest { Name = "Dmitrij S" });

        return result.Message;
    }
}
