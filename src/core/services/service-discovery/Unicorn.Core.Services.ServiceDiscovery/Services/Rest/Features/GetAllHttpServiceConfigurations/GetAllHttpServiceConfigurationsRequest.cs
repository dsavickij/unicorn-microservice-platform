using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetAllHttpServiceConfigurations;

public record GetAllHttpServiceConfigurationsRequest : BaseRequest.RequiringResult<IEnumerable<HttpServiceConfiguration>>
{
}
