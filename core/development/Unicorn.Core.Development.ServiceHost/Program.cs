using Unicorn.Core.Infrastructure.SDK.HostConfiguration;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Services.AddGrpc();

builder.Host.ApplyUnicornConfiguration<ServiceHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares if needed

// Configure the HTTP request pipeline.
app.MapGrpcService<MyGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapControllers();

app.Run();
