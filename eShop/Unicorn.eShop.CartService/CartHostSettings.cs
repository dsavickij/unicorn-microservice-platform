using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.eShop.CartService;

public record CartHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
