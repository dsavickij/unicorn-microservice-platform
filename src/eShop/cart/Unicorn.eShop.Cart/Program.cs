using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.eShop.Cart;
using Unicorn.eShop.Cart.DataAccess;

await ServiceHostBuilder.Build<CartHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, cfg, environament) =>
    {
        services.AddHealthChecks().AddDbContextCheck<CartDbContext>();
        services.AddDatabase(cfg, environament.IsDevelopment());
    });
}).RunAsync();
