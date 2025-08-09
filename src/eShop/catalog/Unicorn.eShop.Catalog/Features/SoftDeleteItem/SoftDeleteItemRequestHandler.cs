using Unicorn.Core.Infrastructure.Communication.SDK.OneWay;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.eShop.Catalog.SDK.Events;

namespace Unicorn.eShop.Catalog.Features.SoftDeleteItem;

public class SoftDeleteItemRequestHandler : BaseHandler.WithResult.ForRequest<SoftDeleteItemRequest>
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
