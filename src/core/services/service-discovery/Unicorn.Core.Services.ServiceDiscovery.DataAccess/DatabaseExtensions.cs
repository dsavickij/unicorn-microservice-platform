using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, string connectionString)
    {
        ConfigureDb(services, connectionString);
      //  SeedDataHelper.AddSeedData(services);
    }

    private static void ConfigureDb(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ServiceDiscoveryDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure());
        });
    }
}
