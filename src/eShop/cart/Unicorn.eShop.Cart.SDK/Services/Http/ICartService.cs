using Refit;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.eShop.Cart.SDK;
using Unicorn.eShop.Cart.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Cart.SDK.Services.Http;

[UnicornRestServiceMarker]
public interface ICartService
{
    [Post("api/carts/{cartId}/items")]
    Task<OperationResult> AddItemAsync(Guid cartId, [Body] CartItemDTO cartItem);

    [Delete("api/carts/{cartId}/items/{itemId}")]
    Task<OperationResult> RemoveItemAsync(Guid cartId, Guid itemId);

    [Get("api/carts/my")]
    Task<OperationResult<CartDTO>> GetMyCartAsync();

    [Get("api/carts/{cartId}/discounts/{discountCode}")]
    Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync(Guid cartId, string discountCode);
}
