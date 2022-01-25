﻿using System.Collections.Concurrent;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

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
            var name = GetAssemlyServiceName(httpServiceInterface);
            var cfg = await _svcDiscoveryClient.GetHttpServiceConfigurationAsync(name);
            _cache.TryAdd(httpServiceInterface.FullName!, cfg);
        }

        return _cache[httpServiceInterface.FullName!];
    }

    private string GetAssemlyServiceName(Type httpServiceInterface)
    {
        var attribute = httpServiceInterface.Assembly.GetCustomAttribute(typeof(UnicornAssemblyServiceNameAttribute));

        if (attribute is UnicornAssemblyServiceNameAttribute nameAttribute)
        {
            return nameAttribute.ServiceName;
        }

        throw new ArgumentException($"Assembly '{httpServiceInterface.Assembly.FullName}' " +
            $"does not include attribute '{typeof(UnicornAssemblyServiceNameAttribute).FullName}'");
    }
}