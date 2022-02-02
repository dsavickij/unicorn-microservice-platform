using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

[UnicornHttpServiceMarker]
public interface ICartService
{
    [UnicornHttpPost("api/carts/{cartId}/items/add")]
    Task<OperationResult> AddItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromBody] CartItemDTO cartItem);

    [UnicornHttpDelete("api/carts/{cartId}/items/{itemId}/remove")]
    Task<OperationResult> RemoveItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromRoute] Guid itemId);

    [UnicornHttpGet("api/carts/my")]
    Task<OperationResult<CartDTO>> GetMyCartAsync();
}
