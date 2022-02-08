using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Discount.SDK.gRPC.Clients;

namespace Unicorn.eShop.Discount.Features.GetCartDiscount;

public record GetCartDiscountRequest : BaseRequest.WithResponse<CartDiscount>
{
    public string DiscountCode { get; set; } = string.Empty;
}
