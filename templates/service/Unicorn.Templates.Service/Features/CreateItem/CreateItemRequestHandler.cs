using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Templates.Service.SDK.DTOs;
using Unicorn.Templates.Service.SDK.Events;

namespace Unicorn.Templates.Service.Features.CreateItem;

public class CreateItemRequestHandler : BaseHandler.WithResult<Item>.For<CreateItemRequest>
{
    private readonly IUnicornEventPublisher _publisher;

    public CreateItemRequestHandler(IUnicornEventPublisher publisher) => _publisher = publisher;

    protected override async Task<OperationResult<Item>> HandleAsync(CreateItemRequest request, CancellationToken cancellationToken)
    {
        // do you magic

        await _publisher.PublishAsync(new ItemCreated { ItemId = Guid.NewGuid() });

        return Ok(new Item());
    }
}
