using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddHealthChecks();

builder.Host.ApplyUnicornConfiguration<ClientHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares if needed

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();