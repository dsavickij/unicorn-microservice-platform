using Playground.ServiceDiscovery.SDK;

namespace Playground.Common.SDK;

public interface IGrpcClientConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcClientName);
}
