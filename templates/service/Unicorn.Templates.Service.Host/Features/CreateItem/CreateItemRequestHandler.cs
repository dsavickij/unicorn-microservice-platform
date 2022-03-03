using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.Templates.Service.SDK.DTOs;

namespace Unicorn.Templates.Service.Features.CreateItem;

public class CreateItemRequestHandler : BaseHandler.WithResult<Item>.For<CreateItemRequest>
{
    protected override async Task<OperationResult<Item>> HandleAsync(CreateItemRequest request, CancellationToken cancellationToken)
    {
        // do you magic

        return Ok(new Item());
    }
}
