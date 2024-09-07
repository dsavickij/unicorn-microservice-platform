using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : BaseRequest.RequiringResult<HttpServiceConfiguration>
{
    public string ServiceHostName { get; set; } = string.Empty;
}
