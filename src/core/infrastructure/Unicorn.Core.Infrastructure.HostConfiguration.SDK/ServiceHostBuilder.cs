using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;
public static class ServiceHostBuilder
{
    public static WebApplication Build<TServiceHostSettings>(string[] args,
        Action<IServiceCollection> serviceCollectionConfiguration, 
        Action<IApplicationBuilder> applicationConfiguration, 
        Action<IEndpointRouteBuilder> endpointRouteConfiguration) where TServiceHostSettings : BaseHostSettings
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc();
        builder.Services.AddHealthChecks();

        serviceCollectionConfiguration(builder.Services);

        builder.Host.ApplyUnicornConfiguration<TServiceHostSettings>();

        var app = builder.Build();

        app.UseUnicorn(app.Environment);

        applicationConfiguration(app);
        endpointRouteConfiguration(app);

        return app;
    }
}
