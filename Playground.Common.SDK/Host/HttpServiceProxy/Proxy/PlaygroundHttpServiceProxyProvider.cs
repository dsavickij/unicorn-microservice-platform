using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Common.SDK.Abstractions.Http;
using Playground.Common.SDK.Host.HttpServiceProxy.Rest;
using Playground.Common.SDK.Host.HttpServiceProxy.Rest.Components;
using Playground.Common.SDK.ServiceDiscovery;
using System.Reflection;

namespace Playground.Common.SDK.Host.HttpServiceProxy.Proxy;

internal class PlaygroundHttpServiceProxyProvider
{
    private readonly IConfiguration _configuration;

    public PlaygroundHttpServiceProxyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    internal void AddHttpServiceProxiesTo(IServiceCollection services)
    {
        var names = GetPlaygroundHttpServiceNames();
        AddHttpServiceProxiesToServiceCollection(names, services);
    }

    private void AddHttpServiceProxiesToServiceCollection(IEnumerable<string> names, IServiceCollection services)
    {
        //var proxyGen = new ProxyGenerator();
        //var svcDiscoveryClient = new ServiceDiscoveryClient(_configuration);
        //var httpServiceCfgProvider = new HttpServiceConfigurationProvider(svcDiscoveryClient);
        //var restRequestProvider = new RestRequestProvider();
        //var restClientProvider = new RestClientProvider(httpServiceCfgProvider);
        //var restComponentProvider = new RestComponentProvider(restClientProvider, restRequestProvider);

        services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>();
        services.AddSingleton<IHttpServiceConfigurationProvider, HttpServiceConfigurationProvider>();
        services.AddTransient<IRestRequestProvider, RestRequestProvider>();
        services.AddTransient<IRestClientProvider, RestClientProvider>();
        services.AddTransient<IRestComponentProvider, RestComponentProvider>();
        services.AddTransient<PlaygroundHttpServiceInterceptor>();

        //foreach (var name in names)
        //{
        //    var loadedType = Type.GetType(name, true);
        //    var instance = proxyGen.CreateInterfaceProxyWithoutTarget(loadedType, new PlaygroundHttpServiceInterceptor(restComponentProvider));

        //    services.AddTransient(loadedType, svcProvider =>
        //    {
        //        var svcDiscoveryProxy = svcProvider.GetService<IServiceDiscoveryService>();

        //    });
        //}

        foreach (var name in names)
        {
            var loadedType = Type.GetType(name, true) ?? throw new Exception(name);

            services.AddTransient(loadedType, serviceProvider =>
            {
                var interceptor = serviceProvider.GetRequiredService<PlaygroundHttpServiceInterceptor>();
                return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(loadedType, interceptor);
            });
        }
    }

    private IEnumerable<string> GetPlaygroundHttpServiceNames()
    {
        using var ctx = GetMedataLoadContext();
        return GetServiceNamesFromAssemblies(ctx);
    }

    private IEnumerable<string> GetServiceNamesFromAssemblies(MetadataLoadContext ctx)
    {
        var markerName = typeof(PlaygroundHttpServiceMarker).AssemblyQualifiedName;
        var allServiceInterfaceNames = new List<string>();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (!assembly.FullName.Contains("Playground")) // change it
                continue;

            var serviceInterfacesNames = assembly
                .GetExportedTypes()
                .Where(t => t.IsInterface && t.GetCustomAttributesData().Any(data => data.AttributeType.AssemblyQualifiedName == markerName))
                .Select(t => t.AssemblyQualifiedName ?? string.Empty);

            allServiceInterfaceNames.AddRange(serviceInterfacesNames);
        }

        return allServiceInterfaceNames;
    }

    private static MetadataLoadContext GetMedataLoadContext()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(object).Assembly;
        var serviceMarkerAssembly = typeof(PlaygroundHttpServiceMarker).Assembly;

        var filePathes = new[] { currentAssembly.Location, coreAssembly.Location, serviceMarkerAssembly.Location };

        return new MetadataLoadContext(new PathAssemblyResolver(filePathes), coreAssembly.GetName().Name);
    }

    private static IEnumerable<string> GetAssemblyFilesFromCurrentDirectory()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        return Directory.GetFiles(path).Where(fileName => fileName.EndsWith(".dll"));
    }
}