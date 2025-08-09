using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateGrpcServiceConfiguration;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateHttpServiceConfiguration;

public class UpdateHttpServiceConfigurationHandler : BaseHandler.WithResult<HttpServiceConfiguration>.ForRequest<UpdateHttpServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;
    private readonly ILogger<CreateGrpcServiceConfigurationHandler> _logger;

    public UpdateHttpServiceConfigurationHandler(ServiceDiscoveryDbContext ctx, ILogger<CreateGrpcServiceConfigurationHandler> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    protected override async Task<OperationResult<HttpServiceConfiguration>> HandleAsync(UpdateHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating HTTP service configuration for service '{request.Configuration.ServiceHostName}'");

        var current = await _ctx.HttpServiceConfigurations
            .SingleAsync(x => x.ServiceHostName == request.Configuration.ServiceHostName);

        current.BaseUrl = request.Configuration.BaseUrl;

        _ctx.HttpServiceConfigurations.Update(current);

        await _ctx.SaveChangesAsync();

        return Ok();
    }
}
