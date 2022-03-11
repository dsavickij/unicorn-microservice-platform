using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.CreateHttpServiceConfiguration;

public record CreateHttpServiceConfigurationRequest : BaseRequest.WithResponse
{
    public HttpServiceConfiguration Configuration { get; set; } = new HttpServiceConfiguration();
}
