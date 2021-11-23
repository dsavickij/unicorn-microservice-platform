using Microsoft.AspNetCore.Mvc;
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

        var result = await _svcDiscoveryService.GetHttpServiceConfiguration("");

        return result;
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

        //var result = await _svcDiscoveryService.GetHttpServiceConfiguration("");

        return new HttpServiceConfiguration();
    }
}
