using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.GrpcServiceClients;

internal interface IGrpcServiceConfigurationProvider
{
    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(MethodInfo grpcServiceMethod);
}

internal class GrpcServiceConfigurationProvider : IGrpcServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, GrpcServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryClient _client;

    public GrpcServiceConfigurationProvider(IServiceDiscoveryClient client) => _client = client;

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(MethodInfo grpcServiceMethod)
    {
        if (grpcServiceMethod.DeclaringType is not null)
        {
            if (!_cache.ContainsKey(grpcServiceMethod.Name))
            {
                var attribute =
                    grpcServiceMethod.DeclaringType.Assembly.GetCustomAttribute(
                        typeof(UnicornServiceHostNameAttribute));

                if (attribute is UnicornServiceHostNameAttribute nameAttribute)
                {
                    (await _client.GetGrpcServiceConfigurationAsync(nameAttribute.ServiceHostName)).Match(
                        result =>
                        {
                            if (result.IsSuccess)
                            {
                                return _cache.TryAdd(grpcServiceMethod.Name, result.Data);
                            }

                            throw new ArgumentException(
                                $"Failed to retrieve Http service configuration for service '{nameAttribute.ServiceHostName}'. " +
                                $"Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                        },
                        _ => throw new InvalidOperationException("No gRPC configuration was retrieved"));
                }
            }

            return _cache[grpcServiceMethod.Name];
        }

        throw new ArgumentException($"Method '{grpcServiceMethod.Name}' belongs to type " +
                                    $"'{grpcServiceMethod.DeclaringType!.FullName}' is not declared in the type in SDK for GRPC services. " +
                                    $"Only GRPC service methods can be used as a delegate");
    }
}