using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;
using Unicorn.eShop.Catalog.SDK;

namespace Unicorn.eShop.Catalog;

public record CatalogHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;

}
