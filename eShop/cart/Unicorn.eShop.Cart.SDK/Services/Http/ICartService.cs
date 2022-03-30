using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.Cart.SDK;
using Unicorn.eShop.Cart.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Cart.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface ICartService
{
    [UnicornHttpPost("api/carts/{cartId}/items")]
    Task<OperationResult> AddItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromBody] CartItemDTO cartItem);

    [UnicornHttpDelete("api/carts/{cartId}/items/{itemId}")]
    Task<OperationResult> RemoveItemAsync([UnicornFromRoute] Guid cartId, [UnicornFromRoute] Guid itemId);

    [UnicornHttpGet("api/carts/my")]
    Task<OperationResult<CartDTO>> GetMyCartAsync();

    [UnicornHttpGet("api/carts/{cartId}/discounts/{discountCode}")]
    Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync([UnicornFromRoute] Guid cartId, [UnicornFromRoute] string discountCode);
}
