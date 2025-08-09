using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : BaseRequest.RequiringResult<HttpServiceConfiguration>
{
    public string ServiceName { get; set; } = string.Empty;
}
