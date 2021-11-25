using System.Collections.Concurrent;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.GrpcServices;

internal interface IGrpcServiceConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcServiceName);
}

internal class GrpcServiceConfigurationProvider : IGrpcServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, GrpcServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryService _client;

    public GrpcServiceConfigurationProvider(IServiceDiscoveryService client)
    {
        _client = client;
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcServiceName)
    {
        if (!_cache.ContainsKey(grpcServiceName))
        {
            var cfg = await _client.GetGrpcServiceConfiguration(grpcServiceName);
            _cache.TryAdd(grpcServiceName, cfg);
        }

        return _cache[grpcServiceName];
    }
}