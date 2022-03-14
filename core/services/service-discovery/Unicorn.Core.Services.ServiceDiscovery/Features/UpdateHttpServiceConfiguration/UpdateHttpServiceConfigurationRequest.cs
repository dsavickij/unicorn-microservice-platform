using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.CreateGrpcServiceConfiguration;

public record UpdateHttpServiceConfigurationRequest : BaseRequest.WithResponse<HttpServiceConfiguration>
{
    public HttpServiceConfiguration Configuration { get; set; } = new HttpServiceConfiguration();
}
