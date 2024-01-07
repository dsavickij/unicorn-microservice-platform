using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.eShop.Discount;
using Unicorn.eShop.Discount.Services.gRPC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// register services on builder.Services if needed

// builder.Services.AddDbContext<CartDbContext>();

builder.Services.AddGrpc();

builder.Host.ApplyUnicornConfiguration<DiscountHostSettings>();

var app = builder.Build();

app.UseUnicorn(app.Environment);

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

// add middlewares here if needed

app.MapGrpcService<DiscountGrpcService>();

app.MapControllers();
app.Run();
