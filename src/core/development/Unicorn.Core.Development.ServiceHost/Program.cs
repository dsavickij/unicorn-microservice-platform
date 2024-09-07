using Unicorn.Core.Development.ServiceHost;
using Unicorn.Core.Development.ServiceHost.SDK.Services.Http;
using MinimalHelpers.OpenApi;
using Unicorn.Core.Development.ServiceHost.SDK;
using Unicorn.Core.Development.ServiceHost.Services.Rest.Films;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceHostBuilder.Build<ServiceHostSettings>(args, builder =>
        {
            builder.WithServiceConfiguration(serviceCollection =>
            {
                serviceCollection.AddTransient<IServiceHostServiceRefit, FilmService>();
                serviceCollection.AddSwaggerGen(options => options.AddFormFile());
                serviceCollection.AddEndpointsApiExplorer();
                serviceCollection.AddAntiforgery();
            })
            .WithEndpointConfiguration(endpointBuilder =>
            {
                endpointBuilder.MapUnicornRestService<IServiceHostServiceRefit>();
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