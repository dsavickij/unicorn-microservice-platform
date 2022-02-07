using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Cart;
using Unicorn.eShop.Cart.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment());

builder.Host.ApplyUnicornConfiguration<CartHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();