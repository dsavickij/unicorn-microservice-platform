using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.GetMyCart;

public record GetMyCartRequest : BaseRequest.WithResponse<CartDTO>
{
}
