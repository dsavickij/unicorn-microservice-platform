using Unicorn.Core.Infrastructure.SDK.HostConfiguration;

var builder = WebApplication.CreateBuilder(args);

// register services on builder.Services if needed

builder.Host.ApplyUnicornConfiguration<ClientHostSettings>();

var app = builder.Build();

app.UseUnicornMiddlewares(app.Environment);

// add middlewares if needed

app.MapControllers();

app.Run();