using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices;

internal static class HttpServiceRegistrationExtensions
{
    internal static void AddHttpServices(this IServiceCollection services, ServiceDiscoverySettings serviceDiscoverySettings)
    {
        services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>(
            _ => new ServiceDiscoveryClient(serviceDiscoverySettings.Url, null)); // TODO: change to real logger instead of null
        services.AddSingleton<IHttpServiceConfigurationProvider, HttpServiceConfigurationProvider>();
        services.AddTransient<IRestRequestProvider, RestRequestProvider>();
        services.AddTransient<IRestClientProvider, RestClientProvider>();
        services.AddTransient<IHttpRequestDispatcher, HttpRequestDispatcher>();
        services.AddTransient<HttpServiceInvocationInterceptor>();

        foreach (var type in GetHttpServiceInterfaceTypes())
        {
            services.AddTransient(type, serviceProvider =>
            {
                var interceptor = serviceProvider.GetRequiredService<HttpServiceInvocationInterceptor>();
                return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type, interceptor);
            });
        }
    }

    private static IEnumerable<Type> GetHttpServiceInterfaceTypes()
    {
        var types = new List<Type>();

        foreach (var name in AssemblyScanner.GetInterfaceNamesDecoratedWith<UnicornHttpServiceMarkerAttribute>())
        {
            var interfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);
            types.Add(interfaceType);
        }

        return types;
    }
}
