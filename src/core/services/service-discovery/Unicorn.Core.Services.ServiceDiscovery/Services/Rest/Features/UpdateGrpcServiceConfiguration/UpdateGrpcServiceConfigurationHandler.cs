using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;
using Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateGrpcServiceConfiguration;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateGrpcServiceConfiguration;

public class UpdateGrpcServiceConfigurationHandler : BaseHandler.WithResult<GrpcServiceConfiguration>.ForRequest<UpdateGrpcServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;
    private readonly ILogger<CreateGrpcServiceConfigurationHandler> _logger;

    public UpdateGrpcServiceConfigurationHandler(ServiceDiscoveryDbContext ctx, ILogger<CreateGrpcServiceConfigurationHandler> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    protected override async Task<OperationResult<GrpcServiceConfiguration>> HandleAsync(
        UpdateGrpcServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating gRPC service configuration for service '{request.Configuration.ServiceHostName}'");

        var current = await _ctx.GrpcServiceConfigurations
            .SingleAsync(x => x.ServiceHostName == request.Configuration.ServiceHostName);

        current.BaseUrl = request.Configuration.BaseUrl;

        _ctx.GrpcServiceConfigurations.Update(current);

        await _ctx.SaveChangesAsync();

        return Ok();
    }
}
