using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http;
using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.Common;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest.Components;

internal interface IHttpServiceConfigurationProvider
{
    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(Type httpServiceInterface);
}

internal class HttpServiceConfigurationProvider : IHttpServiceConfigurationProvider
{
    private readonly ConcurrentDictionary<Type, HttpServiceConfiguration> _cache = new();
    private readonly IServiceDiscoveryClient _svcDiscoveryClient;

    public HttpServiceConfigurationProvider(IServiceDiscoveryClient serviceDiscoveryClient)
    {
        _svcDiscoveryClient = serviceDiscoveryClient;
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfiguration(Type httpServiceInterface)
    {
        if (!_cache.ContainsKey(httpServiceInterface))
        {
            var attribute = GetAssemlyServiceNameAttribute(httpServiceInterface);
            var cfg = await _svcDiscoveryClient.GetHttpServiceConfiguration(attribute.ServiceName);
            _cache.TryAdd(httpServiceInterface, cfg);
        }

        return _cache[httpServiceInterface];
    }

    private PlaygroundAssemblyServiceNameAttribute GetAssemlyServiceNameAttribute(Type httpServiceInterface)
    {
        var attribute = httpServiceInterface.Assembly.GetCustomAttribute(typeof(PlaygroundAssemblyServiceNameAttribute));

        if (attribute is PlaygroundAssemblyServiceNameAttribute nameAttribute)
            return nameAttribute;

        throw new Exception($"Assembly '{httpServiceInterface.Assembly.FullName}' " +
            $"does not include attribute '{typeof(PlaygroundAssemblyServiceNameAttribute).FullName}'");
    }
}