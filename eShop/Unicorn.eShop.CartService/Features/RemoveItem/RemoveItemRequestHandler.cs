using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.Controllers;

namespace Unicorn.eShop.CartService.Features.RemoveItem;

public class RemoveItemRequestHandler : BaseHandler.WithResult.For<RemoveItemRequest>
{
    private readonly CartDbContext _ctx;

    public RemoveItemRequestHandler(CartDbContext context)
    {
        _ctx = context;
    }

    protected override Task<OperationResult> HandleAsync(RemoveItemRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
