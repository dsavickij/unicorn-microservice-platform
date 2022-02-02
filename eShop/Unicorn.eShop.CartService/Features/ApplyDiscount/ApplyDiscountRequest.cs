using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Features.ApplyDiscount;

public record ApplyDiscountRequest : BaseRequest.WithResponse<DiscountedCartDTO>
{
    public Guid CartId { get; set; }
    public string DiscountCode { get; set; } = string.Empty;
}
