using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetAllHttpServiceConfigurations;

public class GetAllHttpServiceConfigurationsRequestHandler : BaseHandler.WithResult<IEnumerable<HttpServiceConfiguration>>.ForRequest<GetAllHttpServiceConfigurationsRequest>
{
    private readonly ServiceDiscoveryDbContext _ctx;

    public GetAllHttpServiceConfigurationsRequestHandler(ServiceDiscoveryDbContext context)
    {
        _ctx = context;
    }

    protected override async Task<OperationResult<IEnumerable<HttpServiceConfiguration>>> HandleAsync(
        GetAllHttpServiceConfigurationsRequest request, CancellationToken cancellationToken)
    {
        var result = await _ctx.HttpServiceConfigurations.ToListAsync();

        return Ok(result.Select(x => new HttpServiceConfiguration
        {
            ServiceHostName = x.ServiceHostName,
            BaseUrl = x.BaseUrl
        }));
    }
}
