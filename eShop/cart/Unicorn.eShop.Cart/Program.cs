using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;
using Unicorn.eShop.Cart;
using Unicorn.eShop.Cart.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks().AddDbContextCheck<CartDbContext>();

// register services on builder.Services if needed

builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment());

builder.Host.ApplyUnicornConfiguration<CartHostSettings>();

var app = builder.Build();

app.UseUnicorn(app.Environment);

app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

// add middlewares here if needed

app.MapControllers();
app.Run();
