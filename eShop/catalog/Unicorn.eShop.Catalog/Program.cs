using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Catalog;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

// builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment());

builder.Host.ApplyUnicornConfiguration<CatalogHostSettings>();

var app = builder.Build();

app.UseUnicorn(app.Environment);

// add middlewares here if needed

app.MapControllers();
app.Run();
