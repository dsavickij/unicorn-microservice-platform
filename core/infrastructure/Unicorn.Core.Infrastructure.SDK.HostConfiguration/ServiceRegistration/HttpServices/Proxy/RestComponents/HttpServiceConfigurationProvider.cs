using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.Common;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices.Proxy.RestComponents;

internal interface IHttpServiceConfigurationProvider
{
    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(Type httpServiceInterface);
}

internal class HttpServiceConfigurationProvider : IHttpServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<string, HttpServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryClient _svcDiscoveryClient;

    public HttpServiceConfigurationProvider(IServiceDiscoveryClient serviceDiscoveryClient)
    {
        _svcDiscoveryClient = serviceDiscoveryClient;
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfiguration(Type httpServiceInterface)
    {
        if (!_cache.ContainsKey(httpServiceInterface.FullName!))
        {
            var name = GetAssemlyServiceName(httpServiceInterface);
            var cfg = await _svcDiscoveryClient.GetHttpServiceConfiguration(name);
            _cache.TryAdd(httpServiceInterface.FullName!, cfg);
        }

        return _cache[httpServiceInterface.FullName!];
    }

    private string GetAssemlyServiceName(Type httpServiceInterface)
    {
        var attribute = httpServiceInterface.Assembly.GetCustomAttribute(typeof(UnicornAssemblyServiceNameAttribute));

        if (attribute is UnicornAssemblyServiceNameAttribute nameAttribute)
            return nameAttribute.ServiceName;

        throw new Exception($"Assembly '{httpServiceInterface.Assembly.FullName}' " +
            $"does not include attribute '{typeof(UnicornAssemblyServiceNameAttribute).FullName}'");
    }
}