using Microsoft.Extensions.DependencyInjection;
using Playground.Common.SDK.Abstractions;
using Playground.Common.SDK.ServiceDiscovery;
using Playground.ServiceDiscovery.SDK;
using System.Collections.Concurrent;
using System.Reflection;

namespace Playground.Common.SDK;

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
        if (!_cache.TryGetValue(httpServiceInterface, out var cfg))
        {
            try
            {
                var attribute = GetAssemlyServiceNameAttribute(httpServiceInterface);
                cfg = await _svcDiscoveryClient.GetHttpServiceConfiguration(attribute.ServiceName);
                _cache.TryAdd(httpServiceInterface, cfg);
            }
            catch (Exception ex)
            {

                throw;
            }
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