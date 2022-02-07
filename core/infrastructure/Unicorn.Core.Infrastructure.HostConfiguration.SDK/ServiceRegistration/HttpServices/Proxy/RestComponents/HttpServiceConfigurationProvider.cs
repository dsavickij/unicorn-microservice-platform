using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

internal interface IHttpServiceConfigurationProvider
{
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(Type httpServiceInterface);
}

internal class HttpServiceConfigurationProvider : IHttpServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, HttpServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryClient _svcDiscoveryClient;

    public HttpServiceConfigurationProvider(IServiceDiscoveryClient serviceDiscoveryClient)
    {
        _svcDiscoveryClient = serviceDiscoveryClient;
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(Type httpServiceInterface)
    {
        if (!_cache.ContainsKey(httpServiceInterface.FullName!))
        {
            var name = GetHostServiceName(httpServiceInterface);
            var cfg = await _svcDiscoveryClient.GetHttpServiceConfigurationAsync(name);
            _cache.TryAdd(httpServiceInterface.FullName!, cfg);
        }

        return _cache[httpServiceInterface.FullName!];
    }

    private string GetHostServiceName(Type httpServiceInterface)
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
