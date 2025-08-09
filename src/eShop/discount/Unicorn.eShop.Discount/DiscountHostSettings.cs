using Unicorn.Core.Infrastructure.Host.SDK.Settings;
using Unicorn.eShop.Discount.SDK;

namespace Unicorn.eShop.Discount;

public record DiscountHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
    public override string ServiceHostName => Constants.ServiceHostName;
}
