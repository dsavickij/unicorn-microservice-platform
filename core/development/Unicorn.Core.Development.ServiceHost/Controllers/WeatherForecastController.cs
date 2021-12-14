using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Development.ServiceHost.SDK;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Common.Operation;

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
    public Task<OperationResult<string>> GetNameAsync()
    {
        return Task.FromResult(new OperationResult<string>(
            OperationStatusCode.Status407ProxyAuthenticationRequired,
             new[] { new OperationError(OperationStatusCode.Status102Processing, "Error") }));

        //    return Task.FromResult(new OperationResult<string>(OperationStatusCode.Status203NonAuthoritative, "test"));
    }

    [HttpGet("GetName/{name}")]
    public Task<OperationResult> GetNameAsync(string name)
    {
        return Task.FromResult(new OperationResult(
            OperationStatusCode.Status410Gone, 
            new OperationError(OperationStatusCode.Status412PreconditionFailed, "Error 2")));
    }

    [HttpPost("Uploadfile/{txt}")]
    public Task<OperationResult<int>> UploadFileAsync(string txt, string second, IFormFile file)
    {
        var ms = new MemoryStream();
        file.CopyTo(ms);

        return Task.FromResult(new OperationResult<int>(OperationStatusCode.Status200OK, 1));
    }

    [HttpPost("Uploadfile2/{txt}")]
    public Task<OperationResult<int>> UploadFileAsync2(string txt, [FromBody] string second)
    {

        return Task.FromResult(new OperationResult<int>(OperationStatusCode.Status200OK, 1));
    }
}
