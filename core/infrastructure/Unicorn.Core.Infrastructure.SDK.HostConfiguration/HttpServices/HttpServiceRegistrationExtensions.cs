using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Playground.Core.Infrastructure.SDK.ServiceCommunication.Http;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.Common;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Proxy;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Proxy.RestComponents;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices;

internal static class HttpServiceRegistrationExtensions
{
    internal static void AddHttpService(this IServiceCollection services)
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

        foreach (var name in AssemblyInspector.GetServiceInterfaceNamesWithAttributeOfType<PlaygroundHttpServiceMarker>())
        {
            var interfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);
            types.Add(interfaceType);
        }

        return types;
    }
}