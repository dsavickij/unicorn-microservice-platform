using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.Core.Infrastructure.Host.SDK.Settings.Defaults;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddHealthChecks();

builder.Host.ApplyUnicornConfiguration<ClientHostSettings>();

var app = builder.Build();

app.UseUnicorn(app.Environment);

// add middlewares if needed

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

app.MapControllers();

app.Run();