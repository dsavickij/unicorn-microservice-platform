using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;
using Unicorn.eShop.Cart.DataAccess;

namespace Unicorn.eShop.Cart;

public record CartHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
