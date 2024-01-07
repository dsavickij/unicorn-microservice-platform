using Unicorn.Core.Development.ServiceHost;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;

ServiceHostBuilder.Build<ServiceHostSettings>(args,
    serviceCollection =>
{
    serviceCollection.AddControllers();
},
applicationBuilder => { },
endpointBuilder =>
{
    endpointBuilder.MapGrpcService<MultiplicationGrpcService>();
    endpointBuilder.MapGrpcService<DivisionGrpcService>();
    endpointBuilder.MapGrpcService<SubtractionGrpcService>();
    endpointBuilder.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);
    endpointBuilder.MapControllers();
}).Run();

//var builder = WebApplication.CreateBuilder(args);

//// register services on builder.Services if needed

//builder.Services.AddGrpc();

//builder.Services.AddHealthChecks();

//builder.Host.ApplyUnicornConfiguration<ServiceHostSettings>();

//var app = builder.Build();

//app.UseUnicorn(app.Environment);

//// add middlewares if needed

//// Configure the HTTP request pipeline.
//app.MapGrpcService<MultiplicationGrpcService>();
//app.MapGrpcService<DivisionGrpcService>();
//app.MapGrpcService<SubtractionGrpcService>();

//app.MapHealthChecks(UnicornSettings.HealthCheck.Pattern, UnicornSettings.HealthCheck.Options);

//// app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//app.MapControllers();

//app.Run();
