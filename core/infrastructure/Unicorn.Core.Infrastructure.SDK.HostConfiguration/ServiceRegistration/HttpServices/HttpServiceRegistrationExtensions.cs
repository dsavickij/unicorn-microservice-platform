using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.Common;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices.Proxy;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices.Proxy.RestComponents;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices;

internal static class HttpServiceRegistrationExtensions
{
    internal static void AddHttpServices(this IServiceCollection services)
    {
        services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>();
        services.AddSingleton<IHttpServiceConfigurationProvider, HttpServiceConfigurationProvider>();
        services.AddTransient<IRestRequestProvider, RestRequestProvider>();
        services.AddTransient<IRestClientProvider, RestClientProvider>();
        services.AddTransient<IRestComponentProvider, RestComponentProvider>();
        services.AddTransient<ServiceInvocationInterceptor>();

        foreach (var type in GetHttpServiceInterfaceTypes())
        {
            services.AddTransient(type, serviceProvider =>
            {
                var interceptor = serviceProvider.GetRequiredService<ServiceInvocationInterceptor>();
                return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type, interceptor);
            });
        }
    }

    private static IEnumerable<Type> GetHttpServiceInterfaceTypes()
    {
        var types = new List<Type>();

        foreach (var name in AssemblyInspector.GetServiceInterfaceNamesWithAttributeOfType<UnicornHttpServiceMarker>())
        {
            var interfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);
            types.Add(interfaceType);
        }

        return types;
    }
}