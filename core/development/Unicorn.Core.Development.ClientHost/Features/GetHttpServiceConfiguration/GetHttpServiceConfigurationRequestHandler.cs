using MediatR;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public class GetHttpServiceConfigurationRequestHandler : IRequestHandler<GetHttpServiceConfigurationRequest, HttpServiceConfiguration>
{
    private readonly ILogger<GetHttpServiceConfigurationRequestHandler> _logger;

    public GetHttpServiceConfigurationRequestHandler(ILogger<GetHttpServiceConfigurationRequestHandler> logger)
    {
        _logger = logger;
    }
    
    public Task<HttpServiceConfiguration> Handle(GetHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"Executing GetWeatherForecast at {DateTime.UtcNow}");
        _logger.LogDebug("Debug msg");
        _logger.LogInformation("Info message");
        _logger.LogWarning("Warning message");
        _logger.LogError("Error at 123");

        return Task.FromResult(new HttpServiceConfiguration { Name = "Name", BaseUrl = "Url" });
    }
}
