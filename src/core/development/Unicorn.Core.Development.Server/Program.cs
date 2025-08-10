using Unicorn.Core.Development.Server;
using Unicorn.Core.Development.Server.SDK.Services.Rest;
using Unicorn.Core.Development.Server.Services.Rest.Films;
using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;

await ServiceHostBuilder.Build<ServiceSettings>(args, builder =>
{
    builder
        .WithServiceConfiguration((services, _, _) =>
        {
            services.AddTransient<IServiceHostService, FilmService>();
        })
        .WithEndpointConfiguration(endpointBuilder =>
        {
            endpointBuilder.MapUnicornRestService<IServiceHostService>();
        })
        .WithApplicationConfiguration(applicationBuilder =>
        {
            // add application configuration
        });
}).RunAsync();