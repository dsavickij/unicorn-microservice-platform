using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.eShop.Catalog;

public record CatalogHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
