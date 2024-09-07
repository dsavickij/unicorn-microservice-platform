using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.eShop.Cart;
using Unicorn.eShop.Cart.DataAccess;

ServiceHostBuilder.Build<CartHostSettings>(args, builder =>
{
    builder.WithServiceConfiguration((services, cfg, environament) =>
    {
        services.AddHealthChecks().AddDbContextCheck<CartDbContext>();
        services.AddDatabase(cfg, environament.IsDevelopment());
    });
}).Run();
