using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

// [Authorize]
public class CartController : UnicornBaseController<ICartService>, ICartService
{
    private readonly ILogger<CartController> _logger;
    private readonly CartDbContext _ctx;

    public CartController(ILogger<CartController> logger, CartDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpPost("api/carts/{cartId}/items/add")]
    public async Task<OperationResult> AddItemAsync([FromRoute] Guid cartId, [FromBody] CartItemDTO cartItem)
    {
        return await SendAsync(new AddItemRequest
        {
            CartId = cartId,
            Item = cartItem
        });
    }

    [HttpGet("api/carts/my")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
        return await SendAsync(new GetMyCartRequest());
    }

    [HttpDelete("api/carts/{cartId}/items/{itemId}/remove")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid cartId, [FromRoute] Guid itemId)
    {
        return await SendAsync(new RemoveItemRequest 
        { 
            CartId = cartId, 
            CatalogItemId = itemId 
        });
    }
}
