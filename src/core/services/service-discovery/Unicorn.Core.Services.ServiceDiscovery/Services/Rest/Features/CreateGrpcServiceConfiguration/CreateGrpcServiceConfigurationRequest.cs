using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.ServiceDiscovery.Services.Rest.Features.CreateGrpcServiceConfiguration;

public record CreateGrpcServiceConfigurationRequest : BaseRequest.RequiringResult
{
    public GrpcServiceConfiguration Configuration { get; set; } = new GrpcServiceConfiguration();
}
