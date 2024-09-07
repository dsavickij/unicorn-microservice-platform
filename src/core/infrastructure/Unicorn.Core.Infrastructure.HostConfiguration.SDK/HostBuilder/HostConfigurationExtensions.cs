﻿using MassTransit.AzureServiceBusTransport.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Logging;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Middlewares;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.GrpcServiceClients;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Validation;
using Unicorn.Core.Infrastructure.Security.IAM;
using Unicorn.Core.Infrastructure.Security.IAM.Middlewares;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;

public static class HostConfigurationExtensions
{

    private static BaseHostSettings HostSettings { get; set; }

    public static void ApplyUnicornConfiguration<THostSettings>(this IHostBuilder builder)
        where THostSettings : BaseHostSettings
    {
        builder
            .ConfigureServices((ctx, services) => services.ConfigureHostSettings<THostSettings>(ctx))
            .ConfigureServices((_, services) => services.ConfigureAuthentication(HostSettings.AuthenticationSettings))
            .ConfigureServices((_, services) => services.ConfigureServices())
            .ConfigureServices((_, services) => services.RegisterInServiceDiscoveryService())
            .UseDefaultServiceProvider((ctx, options) => options.ConfigureServiceProvider())
            .ConfigureLogging(cfg => cfg.ConfigureLogging());
    }

    public static WebApplication UseUnicorn(this WebApplication builder, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseDeveloperExceptionPage();
        }

        builder.UseSwagger();
        builder.UseSwaggerUI(UnicornSwaggerSettings.UIOptions);

        //builder.UseAuthentication();
        //builder.UseAuthorization();

        builder.UseUnicornOperationContext();
        builder.UseMiddleware<ValidationExceptionHandlingMiddleware>();

        return builder;
    }

    private static void RegisterInServiceDiscoveryService(this IServiceCollection services)
    {
        services.AddHostedService<ServiceHostSelfRegististrationWorker>();
    }

    private static void ConfigureHostSettings<THostSettings>(this IServiceCollection services, HostBuilderContext ctx)
        where THostSettings : BaseHostSettings
    {
        // TODO: investigate ConfigureOptions<T>
        services.Configure<THostSettings>(ctx.Configuration.GetSection(typeof(THostSettings).Name));

        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<THostSettings>>();

        if (BaseHostSettingsValidator.DoesNotContainEmptyStrings(settings.Value))
        {
            InternalBaseHostSettings.ServiceHostName = settings.Value.ServiceHostName;
            InternalBaseHostSettings.ServiceDiscoverySettings = settings.Value.ServiceDiscoverySettings;
            InternalBaseHostSettings.AuthenticationSettings = settings.Value.AuthenticationSettings;
            InternalBaseHostSettings.OneWayCommunicationSettings = settings.Value.OneWayCommunicationSettings;

            HostSettings = settings.Value;

            // serviceHostName is registered in service collection to register service in Service Discovery service
            ctx.Configuration[$"{typeof(THostSettings).Name}:{nameof(BaseHostSettings.ServiceHostName)}"] = settings.Value.ServiceHostName;
            services.Configure<ServiceDiscoverySettings>(ctx.Configuration.GetSection($"{typeof(THostSettings).Name}:{nameof(BaseHostSettings.ServiceHostName)}"));
        }
    }

    private static void ConfigureServiceProvider(this ServiceProviderOptions options)
    {
        options.ValidateOnBuild = true;
        options.ValidateScopes = true; // check it
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddApplicationInsightsTelemetry();
        services.AddMediatorComponents();
        services.AddHttpServices();
        services.AddGrpcClients();
        services.ConfigureSwagger();
        services.RegisterControllers();

        // TODO: bring back support for message broker
        //services.AddMessageBroker(cfg =>
        //{
        //    cfg.Type = MessageBrokerType.RabbitMq;
        //    cfg.ConnectionString = HostSettings.OneWayCommunicationSettings.ConnectionString;
        //    cfg.SubscriptionId = HostSettings.OneWayCommunicationSettings.SubscriptionId;
        //    cfg.OneWayMethods = AssemblyScanner.GetOneWayMethodConfigurations();
        //});
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(UnicornSwaggerSettings.GetSwaggerGenOptions(HostSettings.AuthenticationSettings.AuthorityUrl));
    }

    // By default, controllers are not registered in dependency conainer, but we need them to here to be able
    // to call on one way methods defined in them
    private static void RegisterControllers(this IServiceCollection services)
    {
        foreach (var controller in AssemblyScanner.GetUnicornControllers())
        {
            services.AddTransient(controller);
        }
    }
}
