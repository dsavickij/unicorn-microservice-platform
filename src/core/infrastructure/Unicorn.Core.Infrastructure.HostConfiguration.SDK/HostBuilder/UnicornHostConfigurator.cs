using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;

public interface IUnicornHostConfigurator
{
    public IUnicornHostConfigurator WithServiceConfiguration(Action<IServiceCollection, ConfigurationManager, IWebHostEnvironment> serviceCollectionConfiguration);
    public IUnicornHostConfigurator WithApplicationConfiguration(Action<IApplicationBuilder> serviceCollectionConfiguration);
    public IUnicornHostConfigurator WithEndpointConfiguration(Action<IEndpointRouteBuilder> serviceCollectionConfiguration);
}

public class UnicornHostConfigurator : IUnicornHostConfigurator
{
    internal Action<IApplicationBuilder>? ApplicationConfiguration { get; private set; }

    internal Action<IEndpointRouteBuilder>? EndpointConfiguration { get; private set; }

    internal Action<IServiceCollection, ConfigurationManager, IWebHostEnvironment>? ServiceCollectionConfiguration { get; private set; }

    public IUnicornHostConfigurator WithApplicationConfiguration(Action<IApplicationBuilder> applicationConfiguration)
    {
        ApplicationConfiguration = applicationConfiguration ?? throw new ArgumentNullException(""); ;
        return this;
    }

    public IUnicornHostConfigurator WithEndpointConfiguration(Action<IEndpointRouteBuilder> endpointConfiguration)
    {
        EndpointConfiguration = endpointConfiguration ?? throw new ArgumentNullException(""); ;
        return this;
    }

    public IUnicornHostConfigurator WithServiceConfiguration(Action<IServiceCollection, ConfigurationManager, IWebHostEnvironment> serviceCollectionConfiguration)
    {
        ServiceCollectionConfiguration = serviceCollectionConfiguration 
            ?? throw new ArgumentNullException("Service host configuration delegate cannot be null");

        return this;
    }
}
