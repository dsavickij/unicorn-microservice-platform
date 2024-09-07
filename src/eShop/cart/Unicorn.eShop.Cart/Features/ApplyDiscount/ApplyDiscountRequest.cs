using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.ApplyDiscount;

public record ApplyDiscountRequest : BaseRequest.RequiringResult<DiscountedCartDTO>
{
    public Guid CartId { get; set; }
    public string DiscountCode { get; set; } = string.Empty;
}
