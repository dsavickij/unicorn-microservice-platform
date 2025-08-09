using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateHttpServiceConfiguration;

public record UpdateHttpServiceConfigurationRequest : BaseRequest.RequiringResult<HttpServiceConfiguration>
{
    public HttpServiceConfiguration Configuration { get; set; } = new HttpServiceConfiguration();
}
