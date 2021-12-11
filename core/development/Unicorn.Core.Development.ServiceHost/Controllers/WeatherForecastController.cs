using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Development.ServiceHost.SDK;

namespace Unicorn.Core.Development.ServiceHost.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase, IDevelopmentHttpService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("GetName")]
    public Task<string> GetNameAsync()
    {
        return Task.FromResult("Dmitrij");
    }

    [HttpGet("GetName/{name}")]
    public Task<string> GetNameAsync(string name)
    {
        return Task.FromResult(name);
    }

    [HttpPost("Uploadfile/{txt}")]
    public Task<int> UploadFileAsync(string txt, string second, IFormFile file)
    {
        var ms = new MemoryStream();
        file.CopyTo(ms);

        return Task.FromResult(1);
    }

    [HttpPost("Uploadfile2/{txt}")]
    public Task<int> UploadFileAsync2(string txt, [FromBody] string second)
    {

        return Task.FromResult(1);
    }
}
