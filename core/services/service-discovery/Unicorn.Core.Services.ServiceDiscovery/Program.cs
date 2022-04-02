using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.Core.Services.ServiceDiscovery;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddDatabase(
    builder.Configuration[$"{nameof(ServiceDiscoveryHostSettings)}:{nameof(ServiceDiscoveryHostSettings.DbConnectionString)}"]);

builder.Services.AddHostedService<InitialDataBackgroundWorker>();

builder.Services.AddHealthChecks().AddDbContextCheck<ServiceDiscoveryDbContext>();

builder.Host.ApplyUnicornConfiguration<ServiceDiscoveryHostSettings>();

var app = builder.Build();

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

app.UseUnicorn(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();
