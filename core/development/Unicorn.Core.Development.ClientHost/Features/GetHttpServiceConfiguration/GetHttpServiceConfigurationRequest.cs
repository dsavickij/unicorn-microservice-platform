using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : BaseRequest.WithResponse<HttpServiceConfiguration>
{
    public string ServiceName { get; set; } = string.Empty;
}
