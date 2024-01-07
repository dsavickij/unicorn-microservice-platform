using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.DataAccess;
using Unicorn.eShop.Cart.DataAccess.Entities;
using Unicorn.eShop.Cart.SDK.DTOs;

namespace Unicorn.eShop.Cart.Features.AddItem;

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

        // TODO: add validation for catalogItem existence in CatalogService

        var cartId = await GetCartIdAsync(request.CartId);
        await AddItemToCartAsync(cartId, request.Item);

        return Ok();
    }

    private async Task AddItemToCartAsync(Guid cartId, CartItemDTO item)
    {
        await _ctx.CartItems.AddAsync(new CartItemEntity
        {
            CartId = cartId,
            CatalogItemId = item.CatalogItemId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            IsAvailable = true
        });

        await _ctx.SaveChangesAsync();
    }

    private async Task<Guid> GetCartIdAsync(Guid cartId)
    {
        var cart = await _ctx.Carts.FirstOrDefaultAsync(x => x.Id == cartId);

        if (cart is null)
        {
            cart = new CartEntity { Id = cartId }; // TODO: userId should be added here too

            await _ctx.Carts.AddAsync(cart);
            await _ctx.SaveChangesAsync();
        }

        return cart.Id;
    }
}
