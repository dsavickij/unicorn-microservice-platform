using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;
public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDb(services, configuration);
        SeedDataHelper.AddSeedData(services);
    }

    private static void ConfigureDb(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ServiceDiscoveryDbContext>(opt =>
        {
            opt.UseSqlServer(configuration["ServiceDiscoveryHostSettings:DbConnectionString"]);
        });
    }
}
