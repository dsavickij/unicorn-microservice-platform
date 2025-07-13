using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
public static class ServiceHostBuilder
{
    public static WebApplication Build<TServiceHostSettings>(
        string[] args,
        Action<IUnicornHostConfigurator> serviceCollectionConfiguration)
        where TServiceHostSettings : BaseHostSettings
    {
        var unicornBuilder = new UnicornHostConfigurator();
        serviceCollectionConfiguration.Invoke(unicornBuilder);

        var builder = WebApplication.CreateBuilder(args);

        unicornBuilder.ServiceCollectionConfiguration?.Invoke(builder.Services, builder.Configuration, builder.Environment);

        builder.Services.AddGrpc();
        builder.Services.AddHealthChecks();

        builder.Host.ApplyUnicornConfiguration<TServiceHostSettings>();

        var app = builder.Build();

        unicornBuilder.ApplicationConfiguration?.Invoke(app);
        unicornBuilder.EndpointConfiguration?.Invoke(app);

        app.UseUnicorn(app.Environment);
        app.MapControllers();

        return app;
    }
}