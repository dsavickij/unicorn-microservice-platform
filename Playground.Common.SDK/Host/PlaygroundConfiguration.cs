using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Playground.Common.SDK.Host.HttpServiceProxy.Proxy;

namespace Playground.Common.SDK.Host;

public static class PlaygroundConfiguration
{
    public static void ApplyPlaygroundConfiguration(this IHostBuilder builder)
    {
        builder
            .ConfigureServices((ctx, services) => ConfigureService(ctx, services))
            .UseDefaultServiceProvider((ctx, options) => ConfigureServiceProvider(options))
            .ConfigureLogging(cfg => ConfigureLogging(cfg));
    }

    private static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder.AddConsole();
        builder.AddDebug();
        builder.SetMinimumLevel(LogLevel.Trace);
    }

    private static void ConfigureServiceProvider(ServiceProviderOptions options)
    {
        options.ValidateOnBuild = true;
        options.ValidateScopes = true; //??????
    }

    private static void ConfigureService(HostBuilderContext ctx, IServiceCollection services)
    {
        //services.AddTransient<IGrpcClientFactory, GrpcClientFactory>();
        //services.AddSingleton<IGrpcClientConfigurationProvider, GrpcClientConfigurationProvider>();
        //services.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>(_ => CreateServiceDiscoveryClient(ctx.Configuration));
        services.AddHttpServiceProxies(ctx.Configuration);
    }

    //private static ServiceDiscoveryClient CreateServiceDiscoveryClient(IConfiguration configuration)
    //{
    //    var baseUrl = configuration["ServiceDiscoveryServiceUrl"];

    //    if (baseUrl is null)
    //        throw new ArgumentNullException("ServiceDiscoveryServiceUrl");

    //    return new ServiceDiscoveryClient(baseUrl);
    //}

    private static void AddHttpServiceProxies(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            new PlaygroundHttpServiceProxyProvider(configuration).AddHttpServiceProxiesTo(services);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
