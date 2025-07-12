using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Development.ServiceHost;
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
                // services.AddSwaggerGen(options => options.AddFormFile());
                services.AddSwaggerGen();
                services.AddEndpointsApiExplorer();
                // services.AddAntiforgery();
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
                // applicationBuilder.UseAntiforgery();
            });
        }).Run();
    }
}

public record UploadDto(string Name, string Email);

public class AntiForgeryFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Skip anti-forgery validation
        return await next(context);
    }
}