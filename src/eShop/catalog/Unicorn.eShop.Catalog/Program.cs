using Unicorn.Core.Infrastructure.Host.SDK.HostBuilder;
using Unicorn.eShop.Catalog;

await ServiceHostBuilder
    .Build<CatalogHostSettings>(args, builder => { })
    .RunAsync();