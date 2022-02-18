using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Catalog.SDK.Events;

namespace Unicorn.eShop.Catalog.Features.SoftDeleteItem;

public class SoftDeleteItemRequestHandler : BaseHandler.WithResult.For<SoftDeleteItemRequest>
{
    private readonly IUnicornEventPublisher _publisher;

    public SoftDeleteItemRequestHandler(IUnicornEventPublisher publisher)
    {
        _publisher = publisher;
    }

    protected override async Task<OperationResult> HandleAsync(SoftDeleteItemRequest request, CancellationToken cancellationToken)
    {
        await _publisher.PublishAsync(new CatalogItemSoftDeleted { Id = request.Id });

        return Ok();
    }
}
