using MassTransit;
using Microsoft.EntityFrameworkCore;
using Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Abstractions;
using Unicorn.eShop.Cart.DataAccess;
using Unicorn.eShop.Catalog.SDK.Events;

namespace Unicorn.eShop.Cart.EventHandlers;

public class CatalogItemSoftDeletedEventHandler : IUnicornEventHandler<CatalogItemSoftDeleted>
{
    private readonly CartDbContext _ctx;

    public CatalogItemSoftDeletedEventHandler(CartDbContext context)
    {
        _ctx = context;
    }

    public async Task Consume(ConsumeContext<CatalogItemSoftDeleted> context)
    {
        const int take = 100;
        var skip = 0;

        while (true)
        {
            var items = await _ctx.CartItems
                .Where(x => x.CatalogItemId == context.Message.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            if (items.Count is not 0)
            {
                items.ForEach(x => x.IsAvailable = false);
                await _ctx.SaveChangesAsync();

                skip += items.Count;
                continue;
            }

            break;
        }
    }
}
