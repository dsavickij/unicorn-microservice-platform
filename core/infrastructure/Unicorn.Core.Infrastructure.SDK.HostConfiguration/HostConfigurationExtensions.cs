using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcServiceClients;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.HttpServices;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.Settings;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.Settings.Defaults;
using Unicorn.Core.Infrastructure.Security.IAM;
using Unicorn.Core.Infrastructure.Security.IAM.Middlewares;
using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration;

public static class HostConfigurationExtensions
{
    public static void ApplyUnicornConfiguration<THostConfiguration>(this IHostBuilder builder)
        where THostConfiguration : BaseHostSettings
    {
        builder
            .ConfigureServices((ctx, services) => services.ConfigureHostSettings<THostConfiguration>(ctx))
            .ConfigureServices((ctx, services) => services.ConfigureAuthentication(ctx))
            .ConfigureServices((_, services) => services.ConfigureServices())
            .UseDefaultServiceProvider((ctx, options) => options.ConfigureServiceProvider())
            .ConfigureLogging(cfg => cfg.ConfigureLogging());
    }

    public static IApplicationBuilder UseUnicornMiddlewares(this IApplicationBuilder builder, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(UnicornSwaggerSettings.UIOptions);
        }

        builder.UseHttpsRedirection();
        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseUnicornOperationContext();

        return builder;
    }

    private static void ConfigureHostSettings<THostSettings>(this IServiceCollection services, HostBuilderContext ctx)
        where THostSettings : BaseHostSettings
    {
        services.Configure<THostSettings>(ctx.Configuration.GetSection(typeof(THostSettings).Name));
    }

    private static void ConfigureLogging(this ILoggingBuilder builder)
    {
        builder.AddConsole();
        builder.AddDebug();
        builder.SetMinimumLevel(LogLevel.Trace);
    }

    private static void ConfigureServiceProvider(this ServiceProviderOptions options)
    {
        options.ValidateOnBuild = true;
        options.ValidateScopes = true; // check it
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddHttpServices();
        services.AddGrpcClients();
        services.ConfigureSwagger();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        var cfg = Guard.Against.Null(
          services.BuildServiceProvider().GetRequiredService<IOptions<AuthenticationSettings>>(),
          nameof(AuthenticationSettings)).Value;

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(UnicornSwaggerSettings.GetSwaggerGenOptions(cfg.AuthorityUrl));
    }
}
