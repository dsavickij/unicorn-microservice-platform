using Unicorn.Core.Infrastructure.HostConfiguration.SDK.HostBuilder;
using Unicorn.eShop.Catalog;

ServiceHostBuilder.Build<CatalogHostSettings>(args, builder => { }).Run();
