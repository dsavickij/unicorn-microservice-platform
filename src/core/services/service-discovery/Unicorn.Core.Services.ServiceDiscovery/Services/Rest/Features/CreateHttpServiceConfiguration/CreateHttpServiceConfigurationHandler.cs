using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateHttpServiceConfiguration;

public class CreateHttpServiceConfigurationHandler : BaseHandler.WithResult.ForRequest<CreateHttpServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;
    private readonly ILogger<CreateHttpServiceConfigurationHandler> _logger;

    public CreateHttpServiceConfigurationHandler(ServiceDiscoveryDbContext ctx, ILogger<CreateHttpServiceConfigurationHandler> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    protected override async Task<OperationResult> HandleAsync(CreateHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating new HTTP service configuration for service '{request.Configuration.ServiceHostName}'");

        await _ctx.HttpServiceConfigurations.AddAsync(new HttpServiceConfigurationEntity
        {
            ServiceHost = new ServiceHostEntity { Name = request.Configuration.ServiceHostName },
            ServiceHostName = request.Configuration.ServiceHostName,
            BaseUrl = request.Configuration.BaseUrl,
        });

        await _ctx.SaveChangesAsync();

        return Ok();
    }
}
