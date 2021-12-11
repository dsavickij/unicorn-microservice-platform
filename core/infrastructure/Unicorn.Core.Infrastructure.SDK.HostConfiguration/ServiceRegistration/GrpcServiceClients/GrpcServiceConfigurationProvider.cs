using System.Collections.Concurrent;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcServiceClients;

internal interface IGrpcServiceConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcServiceName);
}

internal class GrpcServiceConfigurationProvider : IGrpcServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, GrpcServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryClient _client;

    public GrpcServiceConfigurationProvider(IServiceDiscoveryClient client) => _client = client;

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcServiceName)
    {
        if (!_cache.ContainsKey(grpcServiceName))
        {
            // TODO: if cfg is null throw exception
            var cfg = await _client.GetGrpcServiceConfigurationAsync(grpcServiceName);
            _cache.TryAdd(grpcServiceName, cfg);
        }

        return _cache[grpcServiceName];
    }
}
