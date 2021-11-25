using Playground.Core.Services.ServiceDiscovery.SDK;
using Playground.Core.Services.ServiceDiscovery.SDK.Configurations;
using System.Collections.Concurrent;

namespace Playground.Core.Infrastructure.SDK.HostConfiguration.GrpcServices;

internal interface IGrpcClientConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string grpcClientName);
}

internal class GrpcClientConfigurationProvider : IGrpcClientConfigurationProvider
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