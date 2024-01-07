using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;
using Unicorn.eShop.Cart.SDK;

namespace Unicorn.eShop.Cart;

public record CartHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
