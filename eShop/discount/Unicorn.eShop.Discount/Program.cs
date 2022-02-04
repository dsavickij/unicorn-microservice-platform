using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Discount.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

//builder.Services.AddDbContext<CartDbContext>();

builder.Services.AddGrpc();

//if (builder.Environment.IsDevelopment())
//{
//    // Register the worker responsible of seeding the database with the sample data
//    // Note: in a real world application, this step should be part of a setup script.
//    builder.Services.AddHostedService<SeedDataWorker>();
//}

builder.Host.ApplyUnicornConfiguration<DiscountHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares here if needed

app.MapGrpcService<DiscountGrpcService>();

//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapControllers();
app.Run();