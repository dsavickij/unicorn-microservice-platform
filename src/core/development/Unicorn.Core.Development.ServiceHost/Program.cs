using Unicorn.Core.Development.ServiceHost;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;

ServiceHostBuilder.Build<ServiceHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, _, _) =>
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
}).Run();