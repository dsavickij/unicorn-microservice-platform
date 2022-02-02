using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.Controllers;
using Unicorn.eShop.CartService.Entities;

namespace Unicorn.eShop.CartService.Features.RemoveItem;

public class RemoveItemRequestHandler : BaseHandler.WithResult.For<RemoveItemRequest>
{
    private readonly CartDbContext _ctx;

    public RemoveItemRequestHandler(CartDbContext context)
    {
        _ctx = context;
    }

    protected override async Task<OperationResult> HandleAsync(RemoveItemRequest request, CancellationToken cancellationToken)
    {
        var result = await RemoveItemAsync(request);

        return result.Match(
            notFound => NotFound(),
            success => Ok());
    }

    private async Task<OneOf<NotFound, Success>> RemoveItemAsync(RemoveItemRequest request)
    {
        var item = await GetCartItemAsync(request.CartId, request.CatalogItemId);

        return item.IsT0 ? item.AsT0 : await RemoveItemAsync(item.AsT1);
    }

    private async Task<Success> RemoveItemAsync(CartItem item)
    {
        _ctx.CartItems.Remove(item);
        await _ctx.SaveChangesAsync();

        return new Success();
    }

    private async Task<OneOf<NotFound, CartItem>> GetCartItemAsync(Guid cartId, Guid catalogItemId)
    {
        var result = await _ctx.CartItems.SingleOrDefaultAsync(x => x.CartId == cartId && x.CatalogItemId == catalogItemId);

        return result is null ? new NotFound() : result;
    }
}
