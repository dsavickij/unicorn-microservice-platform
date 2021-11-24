using Microsoft.AspNetCore.Mvc;
using Playground.Common.SDK.Abstractions;
using Playground.ServiceDiscovery.SDK;

namespace Playground.Core.ProofOfConcepts.TestingHost.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IServiceDiscoveryService _svcDiscoveryService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceDiscoveryService serviceDiscoveryService)
    {
        _logger = logger;
        _svcDiscoveryService = serviceDiscoveryService;
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

        var result = await _svcDiscoveryService.GetHttpServiceConfiguration("ccccc");

        //var result = await _svcDiscoveryService
        //    .CreateHttpServiceConfiguration("test", new HttpServiceConfiguration { Name = "CreateHttpServiceConfiguration" });

        //var result = await _svcDiscoveryService
        //    .UpdateHttpServiceConfiguration("newName", new HttpServiceConfiguration { Name = "test" });

        return result;

        //await _svcDiscoveryService.DeleteHttpServiceConfiguration("test");

        //return new HttpServiceConfiguration();

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

        return await _svcDiscoveryService.GetHttpServiceConfiguration("ddd");
    }

    [GrpcClientMarker]
    public interface IMyGrpcServiceClient
    {
        Task<string> MyAsyncEndpoint();
    }

    public class MyGrpcServiceClient : BaseGrpcClient, IMyGrpcServiceClient
    {
        public MyGrpcServiceClient(IGrpcClientFactory factory) : base(factory)
        {
        }

        protected override string GrpcServiceName => nameof(MyGrpcServiceClient);

        public Task<string> MyAsyncEndpoint()
        {
            throw new NotImplementedException();
        }
    }
}
