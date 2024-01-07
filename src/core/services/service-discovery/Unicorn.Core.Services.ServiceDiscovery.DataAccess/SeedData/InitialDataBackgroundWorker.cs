using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unicorn.Core.Services.ServiceDiscovery.SDK;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public class InitialDataBackgroundWorker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _cfg;

    public InitialDataBackgroundWorker(IServiceProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _cfg = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ServiceDiscoveryDbContext>();

        await ctx.Database.MigrateAsync();

        await AddServiceHostNamesAsync(ctx);
        await AddHttpServiceConfigurationsAsync(ctx);
    }

    private async Task AddHttpServiceConfigurationsAsync(ServiceDiscoveryDbContext ctx)
    {
        await AddServiceDiscoveryHttpConfigurationAsync(ctx);
    }

    private async Task AddServiceDiscoveryHttpConfigurationAsync(ServiceDiscoveryDbContext ctx)
    {
        var urls = _cfg["ASPNETCORE_URLS"]?.Split(";") ?? throw new Exception(); // TODO: fix exception throwing;
        var httpUrl = urls.Single(url => url.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

        var current = await ctx.HttpServiceConfigurations.AsNoTracking().FirstOrDefaultAsync(x => x.ServiceHostName == Constants.ServiceHostName);
        var newer = SeedData.HttpServiceConfigurations.ServiceDiscovery with { BaseUrl = httpUrl };

        if (current is null)
        {
            await ctx.HttpServiceConfigurations.AddAsync(newer);
        }
        else
        {
            ctx.HttpServiceConfigurations.Update(newer);
        };

        await ctx.SaveChangesAsync();
    }

    private async Task AddServiceHostNamesAsync(ServiceDiscoveryDbContext ctx)
    {
        var serviceHosts = new[]
       {
            SeedData.ServiceHosts.ServiceDiscovery,
        };

        foreach (var serviceHost in serviceHosts)
        {
            var sh = await ctx.ServiceHosts.AsNoTracking().FirstOrDefaultAsync(x => x.Name == serviceHost.Name);

            if (sh is null)
            {
                await ctx.ServiceHosts.AddAsync(serviceHost);
            }
            else if (serviceHosts.All(x => x != sh))
            {
                ctx.ServiceHosts.Update(serviceHost);
            };
        }

        await ctx.SaveChangesAsync();
    }
}
