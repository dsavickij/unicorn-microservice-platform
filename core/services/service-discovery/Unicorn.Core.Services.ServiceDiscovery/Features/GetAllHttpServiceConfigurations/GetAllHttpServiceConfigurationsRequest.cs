using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetHttpServiceConfiguration;

public record GetAllHttpServiceConfigurationsRequest : BaseRequest.WithResponse<IEnumerable<HttpServiceConfiguration>>
{
}
