using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : BaseRequest.RequiringResult<HttpServiceConfiguration>
{
    public string ServiceHostName { get; set; } = string.Empty;
}
