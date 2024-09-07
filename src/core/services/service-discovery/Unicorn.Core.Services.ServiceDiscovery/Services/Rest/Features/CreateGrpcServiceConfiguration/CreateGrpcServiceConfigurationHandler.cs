using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateGrpcServiceConfiguration;

public class CreateGrpcServiceConfigurationHandler : BaseHandler.WithResult.ForRequest<CreateGrpcServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;
    private readonly ILogger<CreateGrpcServiceConfigurationHandler> _logger;

    public CreateGrpcServiceConfigurationHandler(ServiceDiscoveryDbContext ctx, ILogger<CreateGrpcServiceConfigurationHandler> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    protected override async Task<OperationResult> HandleAsync(CreateGrpcServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating new gRPC service configuration for service '{request.Configuration.ServiceHostName}'");

        var serviceHost = await _ctx.ServiceHosts
            .FirstOrDefaultAsync(x => x.Name == request.Configuration.ServiceHostName);

        await _ctx.GrpcServiceConfigurations.AddAsync(new GrpcServiceConfigurationEntity
        {
            ServiceHost = serviceHost ?? new ServiceHostEntity { Name = request.Configuration.ServiceHostName },
            ServiceHostName = request.Configuration.ServiceHostName,
            BaseUrl = request.Configuration.BaseUrl,
        });

        await _ctx.SaveChangesAsync();

        return Ok();
    }
}
