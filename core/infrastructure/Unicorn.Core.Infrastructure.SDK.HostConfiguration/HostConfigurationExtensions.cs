using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcClients;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration;

public static class HostConfigurationExtensions
{
    public static void ApplyUnicornConfiguration(this IHostBuilder builder)
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
        options.ValidateScopes = true; // ??????
    }

    private static void ConfigureService(IServiceCollection services)
    {
        services.AddHttpServices();
        services.AddGrpcClients();
    }
}
