using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess.Initialization;

internal static class SeedDataHelper
{
    public static void AddSeedData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ServiceDiscoveryDbContext>();

        ctx.Database.Migrate();

        SeedServiceHosts(ctx);
        SeedHttpServiceConfigurations(ctx);
        SeedGrpcServiceConfigurations(ctx);

        ctx.SaveChanges();
    }

    private static void SeedGrpcServiceConfigurations(ServiceDiscoveryDbContext ctx)
    {
        var grpcCfgs = new[]
        {
            SeedData.GrpcServiceConfigurations.ServiceHost,
            SeedData.GrpcServiceConfigurations.Discount,
        };

        foreach (var grpcCfg in grpcCfgs)
        {
            if (Queryable.FirstOrDefault(ctx.GrpcServiceConfigurations.AsNoTracking(), x => x.Id == grpcCfg.Id) is null)
            {
                ctx.GrpcServiceConfigurations.Add(grpcCfg);
            }
            else
            {
                ctx.GrpcServiceConfigurations.Update(grpcCfg);
            };
        }
    }

    private static void SeedHttpServiceConfigurations(ServiceDiscoveryDbContext ctx)
    {
        var httpsCfgs = new[]
        {
            SeedData.HttpServiceConfigurations.ServiceDiscovery,
            SeedData.HttpServiceConfigurations.ServiceHost
        };

        foreach (var httpCfg in httpsCfgs)
        {
            if (Queryable.FirstOrDefault(ctx.HttpServiceConfigurations.AsNoTracking(), x => x.Id == httpCfg.Id) is null)
            {
                ctx.HttpServiceConfigurations.Add(httpCfg);
            }
            else
            {
                ctx.HttpServiceConfigurations.Update(httpCfg);
            };
        }
    }

    private static void SeedServiceHosts(ServiceDiscoveryDbContext ctx)
    {
        var serviceHosts = new[]
        {
            SeedData.ServiceHosts.ServiceDiscovery,
            SeedData.ServiceHosts.ServiceHost,
            SeedData.ServiceHosts.Discount,
            SeedData.ServiceHosts.Cart
        };

        foreach (var serviceHost in serviceHosts)
        {
            if (Queryable.FirstOrDefault(ctx.ServiceHosts.AsNoTracking(), x => x.Name == serviceHost.Name) is null)
            {
                ctx.ServiceHosts.Add(serviceHost);
            }
            else
            {
                ctx.ServiceHosts.Update(serviceHost);
            };
        }
    }
}