using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

internal interface IHttpServiceConfigurationProvider
{
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(Type httpServiceInterface);
}

internal class HttpServiceConfigurationProvider : IHttpServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, HttpServiceConfiguration> _cache = new(); // TODO: check MemoryCache
    private readonly IServiceDiscoveryClient _svcDiscoveryClient;

    public HttpServiceConfigurationProvider(IServiceDiscoveryClient serviceDiscoveryClient) =>
        _svcDiscoveryClient = serviceDiscoveryClient;

    public async Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(Type httpServiceInterface)
    {
        if (!_cache.ContainsKey(httpServiceInterface.FullName!))
        {
            var name = GetServiceHostName(httpServiceInterface);

            (await _svcDiscoveryClient.GetHttpServiceConfigurationAsync(name)).Match(
                result =>
                {
                    if (result.IsSuccess)
                    {
                        return _cache.TryAdd(httpServiceInterface.FullName!, result.Data);
                    }

                    throw new ArgumentException(
                        $"Failed to retrieve Http service configuration for service '{name}'. " +
                        $"Errors: {string.Join("; ", result.Errors.Select(x => x.Message))}");
                },
                _ => throw new InvalidOperationException("No Http configuration was retrieved"));
        }

        return _cache[httpServiceInterface.FullName!];
    }

    private static string GetServiceHostName(Type httpServiceInterface)
    {
        var attribute = httpServiceInterface.Assembly.GetCustomAttribute(typeof(UnicornServiceHostNameAttribute));

        if (attribute is UnicornServiceHostNameAttribute nameAttribute)
        {
            return nameAttribute.ServiceHostName;
        }

        throw new ArgumentException($"Assembly '{httpServiceInterface.Assembly.FullName}' " +
                                    $"does not include attribute '{typeof(UnicornServiceHostNameAttribute).FullName}'");
    }
}