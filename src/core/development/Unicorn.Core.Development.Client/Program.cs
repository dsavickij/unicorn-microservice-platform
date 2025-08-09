using Unicorn.Core.Development.Client;
using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.Core.Infrastructure.Host.SDK.Settings.Defaults;

await ServiceHostBuilder.Build<ClientSettings>(args, builder =>
{
    builder
        .WithServiceConfiguration((services, _, _) =>
        {
            services.AddHealthChecks();
        })
        .WithEndpointConfiguration(endpointBuilder =>
        {
            endpointBuilder.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);
            endpointBuilder.MapControllers();
        })
        .WithApplicationConfiguration(applicationBuilder =>
        {
            // add application configuration
        });
}).RunAsync();