using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Discount.SDK.DTOs;

namespace Unicorn.eShop.Discount.Features.GetCartDiscount;

public record GetCartDiscountRequest : BaseRequest.RequiringResult<CartDiscount>
{
    public string DiscountCode { get; set; } = string.Empty;
}
