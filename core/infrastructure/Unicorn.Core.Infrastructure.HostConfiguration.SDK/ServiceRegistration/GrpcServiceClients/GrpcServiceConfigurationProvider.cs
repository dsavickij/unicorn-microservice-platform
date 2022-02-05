using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.GrpcServiceClients;

internal interface IGrpcServiceConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(MethodInfo grpcServiceMethod);
}

internal class GrpcServiceConfigurationProvider : IGrpcServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, GrpcServiceConfiguration> _cache = new ();
    private readonly IServiceDiscoveryClient _client;

    public GrpcServiceConfigurationProvider(IServiceDiscoveryClient client) => _client = client;

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(MethodInfo grpcServiceMethod)
    {
        if (grpcServiceMethod.DeclaringType is not null && !_cache.ContainsKey(grpcServiceMethod.Name))
        {
            var attribute = grpcServiceMethod.DeclaringType.Assembly.GetCustomAttribute(typeof(UnicornServiceHostNameAttribute));

            if (attribute is UnicornServiceHostNameAttribute nameAttribute)
            {
                // TODO: if cfg is null throw exception
                var cfg = await _client.GetGrpcServiceConfigurationAsync(nameAttribute.ServiceHostName);
                _cache.TryAdd(grpcServiceMethod.Name, cfg);
            }

            return _cache[grpcServiceMethod.Name];
        }

        throw new ArgumentException($"Method '{grpcServiceMethod.Name}' is not declared in the type in SDK for GRPC services. " +
            $"Only GRPC service methods can be used as a delegate");
    }
}
