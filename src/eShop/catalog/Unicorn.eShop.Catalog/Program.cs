using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.eShop.Catalog;

ServiceHostBuilder.Build<CatalogHostSettings>(args, builder => { }).Run();
