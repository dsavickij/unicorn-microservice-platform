using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Playground.Common.SDK.Abstractions.Http;
using Playground.Common.SDK.HostConfiguration.HttpServices.Rest;
using Playground.Common.SDK.HostConfiguration.HttpServices.Rest.Components;
using Playground.Common.SDK.ServiceDiscovery;
using System.Reflection;

namespace Playground.Common.SDK.HostConfiguration.HttpServices;

internal static class HttpServiceRegistrationExtensions
{
    internal static void AddHttpServiceProxies(this IServiceCollection services)
    {
        services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>();
        services.AddSingleton<IHttpServiceConfigurationProvider, HttpServiceConfigurationProvider>();
        services.AddTransient<IRestRequestProvider, RestRequestProvider>();
        services.AddTransient<IRestClientProvider, RestClientProvider>();
        services.AddTransient<IRestComponentProvider, RestComponentProvider>();
        services.AddTransient<HttpServiceInterceptor>();

        foreach (var type in GetHttpServiceInterfaceTypes())
        {
            services.AddTransient(type, serviceProvider =>
            {
                var interceptor = serviceProvider.GetRequiredService<HttpServiceInterceptor>();
                return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type, interceptor);
            });
        }
    }

    private static IEnumerable<Type> GetHttpServiceInterfaceTypes()
    {
        var types = new List<Type>();

        foreach (var name in GetHttpServiceInterfaceNames())
        {
            var interfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);
            types.Add(interfaceType);
        }

        return types;
    }

    private static IEnumerable<string> GetHttpServiceInterfaceNames()
    {
        var markerName = typeof(PlaygroundHttpServiceMarker).AssemblyQualifiedName;
        var allServiceInterfaceNames = new List<string>();

        using var ctx = GetMedataLoadContext();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (IsAssemblyPlaygroundServiceAssembly(assembly))
            {
                var serviceInterfacesNames = assembly
                    .GetExportedTypes()
                    .Where(t => t.IsInterface && t.GetCustomAttributesData().Any(
                        data => data.AttributeType.AssemblyQualifiedName == markerName))
                    .Select(t => t.AssemblyQualifiedName ?? string.Empty);

                allServiceInterfaceNames.AddRange(serviceInterfacesNames);
            }
        }

        return allServiceInterfaceNames;
    }

    private static bool IsAssemblyPlaygroundServiceAssembly(Assembly assembly) =>
        assembly.FullName!.Contains("Playground");

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