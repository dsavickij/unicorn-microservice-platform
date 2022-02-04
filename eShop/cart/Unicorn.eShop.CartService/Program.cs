using Microsoft.IdentityModel.Logging;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.CartService;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddDbContext<CartDbContext>();

IdentityModelEventSource.ShowPII = true; //Add this line

if (builder.Environment.IsDevelopment())
{
    // Register the worker responsible of seeding the database with the sample data
    // Note: in a real world application, this step should be part of a setup script.
    builder.Services.AddHostedService<SeedDataWorker>();
}

builder.Host.ApplyUnicornConfiguration<CartHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();