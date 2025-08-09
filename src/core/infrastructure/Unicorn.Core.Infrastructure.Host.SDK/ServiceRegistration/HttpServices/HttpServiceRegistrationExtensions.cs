using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices;

internal static class HttpServiceRegistrationExtensions
{
    internal static void AddHttpServices(this IServiceCollection services)
    {
        services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>();
        services.AddSingleton<IHttpServiceConfigurationProvider, HttpServiceConfigurationProvider>();
        services.AddTransient<IRestClientProvider, RestClientProvider>();
        services.AddTransient<IHttpRequestDispatcher, HttpRequestDispatcher>();
        services.AddTransient<HttpServiceInvocationInterceptor2>();

        foreach (var type in GetHttpServiceInterfaceTypes())
        {
            services.AddTransient(type, serviceProvider =>
            {
                var interceptor = serviceProvider.GetRequiredService<HttpServiceInvocationInterceptor2>();
                return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type, interceptor);
            });
        }
    }

    private static IEnumerable<Type> GetHttpServiceInterfaceTypes()
    {
        var types = new List<Type>();

        foreach (var name in AssemblyScanner.GetInterfaceNamesDecoratedWith<UnicornRestServiceMarkerAttribute>())
        {
            var interfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);
            types.Add(interfaceType);
        }

        return types;
    }
}
