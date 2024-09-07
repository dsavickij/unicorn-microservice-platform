using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.Core.Services.ServiceDiscovery;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

ServiceHostBuilder.Build<ServiceDiscoveryHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, cfg, _) =>
    {
        services.AddDatabase(cfg[$"{nameof(ServiceDiscoveryHostSettings)}:{nameof(ServiceDiscoveryHostSettings.DbConnectionString)}"]);
        services.AddHostedService<InitialDataBackgroundWorker>();
        services.AddHealthChecks().AddDbContextCheck<ServiceDiscoveryDbContext>();
    });
}).Run();
