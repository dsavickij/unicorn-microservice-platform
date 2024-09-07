using Unicorn.Core.Development.ServiceHost;
using MinimalHelpers.OpenApi;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Rest;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceHostBuilder.Build<ServiceHostSettings>(args, builder =>
        {
            builder.WithServiceConfiguration((services, _, _) =>
            {
                services.AddTransient<IServiceHostService, FilmService>();
                services.AddSwaggerGen(options => options.AddFormFile());
                services.AddEndpointsApiExplorer();
                services.AddAntiforgery();
            })
            .WithEndpointConfiguration(endpointBuilder =>
            {
                endpointBuilder.MapUnicornRestService<IServiceHostService>();
                endpointBuilder.MapSwagger();
            })
            .WithApplicationConfiguration(applicationBuilder =>
            {
                applicationBuilder.UseSwaggerUI(x => x.DocumentTitle = Constants.ServiceHostName);
                applicationBuilder.UseSwagger();
                applicationBuilder.UseAntiforgery();
            });
        }).Run();
    }
}