using GrpcService1.SDK.GrpcClients;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using System.Reflection;

namespace WebApplication1;

public static class PlaygroundHostBuilder
{
    public static IHostBuilder CreateBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseConfiguration(GetConfiguration());
             })
             .ConfigureServices(ConfigureServices)
             .UseDefaultServiceProvider(opt =>
             {
                 opt.ValidateOnBuild = true;
                 opt.ValidateScopes = true; //??????
                 });

        return builder;
    }

    private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
    {
        //services.AddGreeterProtoClient(cfg => cfg.Address = "http://localhost:5080");
    }

    private static IConfiguration GetConfiguration()
    {
        var configBuilder = new ConfigurationBuilder();
        return configBuilder.Build();
    }
}

public static class PlaygroundHostBuilder2
{
    public static PlaygroundApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseConfiguration(GetConfiguration());
                     //.UseStartup(Assembly.GetExecutingAssembly().FullName ?? string.Empty);
                 })
             .ConfigureServices(ConfigureServices)
             .UseDefaultServiceProvider(opt =>
             {
                 opt.ValidateOnBuild = true;
                 opt.ValidateScopes = true; //??????
                 });

        return new PlaygroundApplicationBuilder(builder.Build());
    }

    private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
    {
        //services.AddGreeterProtoClient(cfg => cfg.Address = "http://localhost:5080");

        services.AddAuthorization();
        services.AddControllers();
    }

    private static IConfiguration GetConfiguration()
    {
        var configBuilder = new ConfigurationBuilder();
        return configBuilder.Build();
    }
}

public class PlaygroundApplicationBuilder
{
    private IHost _host;

    public PlaygroundApplicationBuilder()
    {

    }

    public PlaygroundApplicationBuilder(IHost host)
    {
        _host = host;
    }

    public MyBuilder Build() => new MyBuilder(_host);
}


public class MyBuilder : IApplicationBuilder, IHost, IEndpointRouteBuilder, IAsyncDisposable
{
    internal const string GlobalEndpointRouteBuilderKey = "__GlobalEndpointRouteBuilder";

    private readonly IHost _host;
    private readonly List<EndpointDataSource> _dataSources = new();
    public ILogger Logger;

    public ApplicationBuilder ApplicationBuilder { get; }

    public MyBuilder(IHost host)
    {
        _host = host;
        ApplicationBuilder = new ApplicationBuilder(host.Services);
        Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("appName");

        Properties[GlobalEndpointRouteBuilderKey] = this;
    }

    public IServiceProvider ApplicationServices
    {
        get => ApplicationBuilder.ApplicationServices;
        set => ApplicationBuilder.ApplicationServices = value;
    }

    public IFeatureCollection ServerFeatures => throw new NotImplementedException();

    public IDictionary<string, object?> Properties => ApplicationBuilder.Properties;

    public IServiceProvider Services => _host.Services;

    public IServiceProvider ServiceProvider => Services;

    public ICollection<EndpointDataSource> DataSources => _dataSources;

    IFeatureCollection IApplicationBuilder.ServerFeatures => _host.Services.GetRequiredService<IServer>().Features;
    public RequestDelegate Build() => ApplicationBuilder.Build();

    public IApplicationBuilder CreateApplicationBuilder() => ((IApplicationBuilder)this).New();

    public void Dispose() => _host.Dispose();

    public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

    public IApplicationBuilder New()
    {
        var newBuilder = ApplicationBuilder.New();
        // Remove the route builder so branched pipelines have their own routing world
        newBuilder.Properties.Remove(GlobalEndpointRouteBuilderKey);
        return newBuilder;
    }

    public Task StartAsync(CancellationToken cancellationToken = default) => _host.StartAsync(cancellationToken);


    public Task StopAsync(CancellationToken cancellationToken = default) => _host.StopAsync(cancellationToken);

    public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        ApplicationBuilder.Use(middleware);
        return this;
    }
}
