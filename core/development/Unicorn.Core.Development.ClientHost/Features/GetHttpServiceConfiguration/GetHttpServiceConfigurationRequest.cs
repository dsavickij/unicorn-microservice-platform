using MediatR;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : IRequest<HttpServiceConfiguration>
{
}
