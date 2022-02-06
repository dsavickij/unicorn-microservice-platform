using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetGrpcServiceConfiguration;

public record GetHttpServiceConfigurationRequest : BaseRequest.WithResponse<HttpServiceConfiguration>
{
    public string ServiceHostName { get; set; } = string.Empty;
}
