using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.Templates.Service;
using Unicorn.Templates.Service.Services.gRPC;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddHealthChecks(); // add health checks for databases, etc.

builder.Services.AddGrpc();

builder.Host.ApplyUnicornConfiguration<YOUR_SERVICE_NAMEHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares if needed

app.MapGrpcService<YOUR_SERVICE_NAMEGrpcService>();

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

app.MapControllers();

app.Run();
