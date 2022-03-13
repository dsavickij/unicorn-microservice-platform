using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.CreateGrpcServiceConfiguration;

public class CreateGrpcServiceConfigurationHandler : BaseHandler.WithResult.For<CreateGrpcServiceConfigurationRequest>
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

        await _ctx.GrpcServiceConfigurations.AddAsync(new GrpcServiceConfigurationEntity
        {
            ServiceHostName = request.Configuration.ServiceHostName,
            BaseUrl = request.Configuration.BaseUrl,
        });

        await _ctx.SaveChangesAsync();

        return Ok();
    }
}
