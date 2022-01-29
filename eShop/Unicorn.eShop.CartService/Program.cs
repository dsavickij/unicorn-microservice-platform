using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.CartService;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddDbContext<CartDbContext>();

builder.Host.ApplyUnicornConfiguration<CartHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();