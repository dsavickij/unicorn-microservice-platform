using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.eShop.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// register services on builder.Services if needed

builder.Host.ApplyUnicornConfiguration<CatalogHostSettings>();

var app = builder.Build();

app.UseUnicorn(app.Environment);

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

// add middlewares here if needed

app.MapControllers();
app.Run();
