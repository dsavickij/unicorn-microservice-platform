using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Playground.Common.SDK.HostConfiguration.GrpcServices;
using Playground.Common.SDK.HostConfiguration.HttpServices;

namespace Playground.Common.SDK.HostConfiguration;

public static class ServiceHostConfigurationExtensions
{
    public static void ApplyPlaygroundConfiguration(this IHostBuilder builder)
    {
        builder
            .ConfigureServices((ctx, services) => ConfigureService(services))
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

    private static void ConfigureService(IServiceCollection services)
    {
        services.AddHttpServiceProxies();
        services.AddGrpcServices();
    }
}
