using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Features.CreateGrpcServiceConfiguration;

public record UpdateGrpcServiceConfigurationRequest : BaseRequest.RequiringResult<GrpcServiceConfiguration>
{
    public GrpcServiceConfiguration Configuration { get; set; } = new GrpcServiceConfiguration();
}
