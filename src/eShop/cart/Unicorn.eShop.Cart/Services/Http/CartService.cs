using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Cart.Features.AddItem;
using Unicorn.eShop.Cart.Features.ApplyDiscount;
using Unicorn.eShop.Cart.Features.GetMyCart;
using Unicorn.eShop.Cart.Features.RemoveItem;
using Unicorn.eShop.Cart.SDK.DTOs;
using Unicorn.eShop.Cart.SDK.Services.Http;

namespace Unicorn.eShop.Cart.Services.Http;

public class CartService : UnicornHttpService<ICartService>, ICartService
{
    private readonly ILogger<CartService> _logger;

    public CartService(ILogger<CartService> logger)
    {
        _logger = logger;
    }

    [HttpPost("api/carts/{cartId}/items")]
    public async Task<OperationResult> AddItemAsync([FromRoute] Guid cartId, [FromBody] CartItemDTO cartItem)
    {
        return await SendAsync(new AddItemRequest
        {
            CartId = cartId,
            Item = cartItem
        });
    }

    [HttpGet("api/carts/{cartId}/discounts/{discountCode}")]
    public async Task<OperationResult<DiscountedCartDTO>> ApplyDiscountAsync(
        [FromRoute] Guid cartId, [FromRoute] string discountCode)
    {
        return await SendAsync(new ApplyDiscountRequest
        {
            CartId = cartId,
            DiscountCode = discountCode
        });
    }

    [HttpGet("api/carts/my")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
        return await SendAsync(new GetMyCartRequest());
    }

    [HttpDelete("api/carts/{cartId}/items/{itemId}")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid cartId, [FromRoute] Guid itemId)
    {
        return await SendAsync(new RemoveItemRequest
        {
            CartId = cartId,
            CatalogItemId = itemId
        });
    }
}
