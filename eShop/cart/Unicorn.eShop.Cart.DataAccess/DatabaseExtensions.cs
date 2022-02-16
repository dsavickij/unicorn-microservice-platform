using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.eShop.Cart.DataAccess.SeedData;

namespace Unicorn.eShop.Cart.DataAccess;

public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment)
    {
        services.AddDbContext<CartDbContext>(b =>
{
            b.UseNpgsql(configuration.GetRequiredSection("CartHostSettings:DbConnectionString").Value);
        });

        if (isDevelopmentEnvironment)
        {
            services.AddHostedService<SeedDataWorker>();
        }
    }
}
