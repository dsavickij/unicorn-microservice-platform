using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Abstractions;
using Unicorn.eShop.Catalog.SDK.Events;

namespace Unicorn.eShop.Cart.EventHandlers;

public class CatalogItemSoftDeletedEventHandler : IUnicornEventHandler<CatalogItemSoftDeleted>
{
    public Task Consume(ConsumeContext<CatalogItemSoftDeleted> context)
    {
        return Task.CompletedTask;
    }
}
