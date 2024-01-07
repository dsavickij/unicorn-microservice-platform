using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetGrpcServiceConfiguration;

public class GetGrpcServiceConfigurationRequestHandler : BaseHandler.WithResult<GrpcServiceConfiguration>.For<GetGrpcServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;

    public GetGrpcServiceConfigurationRequestHandler(ServiceDiscoveryDbContext context)
    {
        _ctx = context;
    }

    protected override async Task<OperationResult<GrpcServiceConfiguration>> HandleAsync(
        GetGrpcServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        var result = await _ctx.GrpcServiceConfigurations
            .FirstOrDefaultAsync(x => x.ServiceHostName == request.ServiceHostName);

        if (result is null)
        {
            return NotFound($"Grpc service configuration for service host '{request.ServiceHostName}' was not found");
        }

        return Ok(new GrpcServiceConfiguration
        {
            ServiceHostName = result.ServiceHostName,
            BaseUrl = result.BaseUrl,
            Port = result.Port,
        });
    }
}
