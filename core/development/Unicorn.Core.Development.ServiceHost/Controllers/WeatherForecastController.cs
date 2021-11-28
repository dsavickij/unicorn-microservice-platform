using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IServiceDiscoveryService _svcDiscoveryService;
    private readonly IMyGrpcServiceClient _client;

    public WeatherForecastController(IServiceDiscoveryService serviceDiscoveryService, IMyGrpcServiceClient greeterProtoClient)
    {
        _svcDiscoveryService = serviceDiscoveryService;
        _client = greeterProtoClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<HttpServiceConfiguration> Get()
    {
        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //{
        //    Date = DateTime.Now.AddDays(index),
        //    TemperatureC = Random.Shared.Next(-20, 55),
        //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //})
        //.ToArray();

        var r = await _client.MyAsyncEndpointAsync();

        //var result = await _svcDiscoveryService.GetHttpServiceConfigurationAsync("ccccc");

        ////var result = await _svcDiscoveryService
        ////    .CreateHttpServiceConfiguration("test", new HttpServiceConfiguration { Name = "CreateHttpServiceConfiguration" });

        ////var result = await _svcDiscoveryService
        ////    .UpdateHttpServiceConfiguration("newName", new HttpServiceConfiguration { Name = "test" });

        //return result;

        //await _svcDiscoveryService.DeleteHttpServiceConfiguration("test");

        return new HttpServiceConfiguration();

    }

    [HttpGet("GetWeatherForecast/{name}")]
    public async Task<HttpServiceConfiguration> GetName(string name)
    {
        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //{
        //    Date = DateTime.Now.AddDays(index),
        //    TemperatureC = Random.Shared.Next(-20, 55),
        //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //})
        //.ToArray();

        return await _svcDiscoveryService.GetHttpServiceConfigurationAsync("ddd");
    }

    [UnicornGrpcClientMarker]
    public interface IMyGrpcServiceClient
    {
        Task<string> MyAsyncEndpointAsync();
    }

    public class MyGrpcServiceClient : BaseGrpcClient, IMyGrpcServiceClient
    {
        public MyGrpcServiceClient(IGrpcClientFactory factory) : base(factory)
        {
        }

        protected override string GrpcServiceName => nameof(MyGrpcServiceClient);

        public async Task<string> MyAsyncEndpointAsync()
        {
            var r = await Factory.CallAsync<string>(GrpcServiceName, null);

            return "";
        }
    }
}
