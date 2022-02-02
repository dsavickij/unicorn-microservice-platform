using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.eShop.CartService.Controllers;

public record GetMyCartRequest : BaseRequest.WithResponse<CartDTO>
{
}