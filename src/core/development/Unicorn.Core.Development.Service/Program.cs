using Unicorn.Core.Development.Service;
using Unicorn.Core.Development.Service.Services.Rest.Films;
using Unicorn.Core.Development.ServiceHost;
using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;

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