using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.eShop.Discount;

public record DiscountHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}
