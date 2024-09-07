using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateHttpServiceConfiguration;

public record CreateHttpServiceConfigurationRequest : BaseRequest.RequiringResult
{
    public HttpServiceConfiguration Configuration { get; set; } = new HttpServiceConfiguration();
}
