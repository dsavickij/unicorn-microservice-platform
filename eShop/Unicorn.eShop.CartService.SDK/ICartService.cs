using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

[UnicornHttpServiceMarker]
public interface ICartService
{
    [UnicornHttpPost("api/add-item")]
    Task<OperationResult> AddItemAsync([UnicornFromBody] CartItemDTO cartItem);

    [UnicornHttpDelete("api/remove-item/{itemId}")]
    Task<OperationResult> RemoveItemAsync([UnicornFromRoute] Guid itemId);

    [UnicornHttpGet("api/my-cart")]
    Task<OperationResult<CartDTO>> GetMyCartAsync();
}
