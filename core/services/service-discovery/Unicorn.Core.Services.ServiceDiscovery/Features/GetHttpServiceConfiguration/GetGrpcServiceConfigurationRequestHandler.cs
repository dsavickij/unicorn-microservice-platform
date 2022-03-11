using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetHttpServiceConfiguration;

public class GetHttpServiceConfigurationRequestHandler : BaseHandler.WithResult<HttpServiceConfiguration>.For<GetHttpServiceConfigurationRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;

    public GetHttpServiceConfigurationRequestHandler(ServiceDiscoveryDbContext context)
    {
        _ctx = context;
    }

    protected override async Task<OperationResult<HttpServiceConfiguration>> HandleAsync(
        GetHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        var result = await _ctx.HttpServiceConfigurations
            .FirstOrDefaultAsync(x => x.ServiceHostName == request.ServiceHostName);

        if (result is null)
        {
            return NotFound($"Grpc service configuration for service host '{request.ServiceHostName}' was not found");
        }

        return Ok(new HttpServiceConfiguration
        {
            ServiceHostName = result.ServiceHostName,
            BaseUrl = result.BaseUrl,
        });
    }
}
