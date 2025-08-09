using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.GetMyCart;

public record GetMyCartRequest : BaseRequest.RequiringResult<CartDTO>
{
}
