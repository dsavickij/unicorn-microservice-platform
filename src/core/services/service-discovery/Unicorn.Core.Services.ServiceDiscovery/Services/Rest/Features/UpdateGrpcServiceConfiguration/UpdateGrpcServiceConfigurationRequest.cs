using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.UpdateGrpcServiceConfiguration;

public record UpdateGrpcServiceConfigurationRequest : BaseRequest.RequiringResult<GrpcServiceConfiguration>
{
    public GrpcServiceConfiguration Configuration { get; set; } = new GrpcServiceConfiguration();
}
