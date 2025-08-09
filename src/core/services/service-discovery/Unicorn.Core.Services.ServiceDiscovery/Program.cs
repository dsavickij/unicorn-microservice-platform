using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.Core.Services.ServiceDiscovery;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

await ServiceHostBuilder.Build<ServiceDiscoveryHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, cfg, _) =>
    {
        services.AddDatabase(cfg[$"{nameof(ServiceDiscoveryHostSettings)}:{nameof(ServiceDiscoveryHostSettings.DbConnectionString)}"]);
        services.AddHostedService<InitialDataBackgroundWorker>();
        services.AddHealthChecks().AddDbContextCheck<ServiceDiscoveryDbContext>();
    });
}).RunAsync();
