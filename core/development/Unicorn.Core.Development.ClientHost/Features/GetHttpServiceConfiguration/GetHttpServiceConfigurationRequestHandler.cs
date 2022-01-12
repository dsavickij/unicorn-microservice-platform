using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public class GetHttpServiceConfigurationRequestHandler : BaseHandler<GetHttpServiceConfigurationRequest, HttpServiceConfiguration>
{
    private readonly ILogger<GetHttpServiceConfigurationRequestHandler> _logger;

    public GetHttpServiceConfigurationRequestHandler(ILogger<GetHttpServiceConfigurationRequestHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<OperationResult<HttpServiceConfiguration>> HandleAsync(
        GetHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"Executing GetWeatherForecast at {DateTime.UtcNow}");
        _logger.LogDebug("Debug msg");
        _logger.LogInformation("Info message");
        _logger.LogWarning("Warning message");
        _logger.LogError("Error at 123");

        return Ok(new HttpServiceConfiguration { Name = "Name", BaseUrl = "Url" });
    }
}
