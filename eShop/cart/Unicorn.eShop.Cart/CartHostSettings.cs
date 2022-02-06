using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.eShop.Cart;

public record CartHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
