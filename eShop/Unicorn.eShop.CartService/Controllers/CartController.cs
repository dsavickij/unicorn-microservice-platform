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

    [HttpPost("api/add-item")]
    public async Task<OperationResult> AddItemAsync([FromBody] CartItemDTO cartItem)
    {
        return await SendAsync(new AddItemRequest
        {
            UserId = new Guid("d4787949-5012-47e0-8082-a5da33e8e1df"),
            Item = cartItem
        });
    }

    [HttpGet("api/my-cart")]
    public async Task<OperationResult<CartDTO>> GetMyCartAsync()
    {
        return await Mediator.Send(new GetMyCartRequest());
    }

    [HttpDelete("api/remove-item/{itemId}")]
    public async Task<OperationResult> RemoveItemAsync([FromRoute] Guid itemId)
    {
        return await Mediator.Send(new RemoveItemRequest { ItemId = itemId });
    }
}
