using Playground.ServiceDiscovery.SDK;
using System.Collections.Concurrent;

namespace Playground.Common.SDK;

public class GrpcClientConfigurationProvider : IGrpcClientConfigurationProvider
{
    private readonly ConcurrentDictionary<string, GrpcServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryService _client;

    public GrpcClientConfigurationProvider(IServiceDiscoveryService client)
    {
        _client = client;
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcClientName)
    {
        if (_cache.ContainsKey(grpcClientName))
            return _cache[grpcClientName];

        var cfg = await _client.GetGrpcServiceConfiguration(grpcClientName);

        _cache.TryAdd(grpcClientName, cfg);

        return cfg;
    }
}