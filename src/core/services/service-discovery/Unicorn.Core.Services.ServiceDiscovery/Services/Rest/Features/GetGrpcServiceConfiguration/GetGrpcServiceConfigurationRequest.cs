using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.GetGrpcServiceConfiguration;

public record GetGrpcServiceConfigurationRequest : BaseRequest.RequiringResult<GrpcServiceConfiguration>
{
    public string ServiceHostName { get; set; } = string.Empty;
}
