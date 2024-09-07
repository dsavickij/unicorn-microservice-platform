using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.GetGrpcServiceConfiguration;

public record GetGrpcServiceConfigurationRequest : BaseRequest.RequiringResult<GrpcServiceConfiguration>
{
    public string ServiceHostName { get; set; } = string.Empty;
}
