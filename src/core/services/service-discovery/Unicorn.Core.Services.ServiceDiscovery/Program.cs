using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.Core.Services.ServiceDiscovery;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess.Initialization;

await ServiceHostBuilder.Build<ServiceDiscoverySettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, cfg, _) =>
    {
        services.AddDatabase(cfg[$"{nameof(ServiceDiscoverySettings)}:{nameof(ServiceDiscoverySettings.DbConnectionString)}"]);
        services.AddHostedService<InitialDataBackgroundWorker>();
        services.AddHealthChecks().AddDbContextCheck<ServiceDiscoveryDbContext>();
    });
}).RunAsync();
