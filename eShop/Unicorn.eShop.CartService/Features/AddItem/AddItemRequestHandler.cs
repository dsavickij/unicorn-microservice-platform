using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;
using Unicorn.eShop.CartService.Controllers;
using Unicorn.eShop.CartService.Entities;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Features.AddItem;

public class AddItemRequestHandler : BaseHandler.WithResult.For<AddItemRequest>
{
    private readonly CartDbContext _ctx;

    public AddItemRequestHandler(CartDbContext ctx)
    {
        _ctx = ctx;
    }

    protected override async Task<OperationResult> HandleAsync(AddItemRequest request, CancellationToken cancellationToken)
    {
       // await _ctx.Database.EnsureDeletedAsync();
       // await _ctx.Database.EnsureCreatedAsync();

        //TODO: add validation for catalogItem existence in CatalogService

        var cartId = await GetCartIdAsync(request.UserId);
        await AddItemToCartAsync(cartId, request.Item);

        return Ok();
    }

    private async Task AddItemToCartAsync(Guid cartId, CartItemDTO item)
    {
        await _ctx.CartItems.AddAsync(new CartItem
        {
            CartId = cartId,
            CatalogItemId = item.CatalogItemId,
            Quantity = item.Quantity,
            ItemPrice = item.UnitPrice,
        });

        await _ctx.SaveChangesAsync();
    }

    private async Task<Guid> GetCartIdAsync(Guid userId)
    {   
        var cart = await _ctx.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart is null)
        {
            cart = new Cart { UserId = userId };

            await _ctx.Carts.AddAsync(cart);
            await _ctx.SaveChangesAsync();
        }

        return cart.Id;
    }
}
