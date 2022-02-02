using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Features.ApplyDiscount;

public class ApplyDiscountRequestHandler : BaseHandler.WithResult<DiscountedCartDTO>.For<ApplyDiscountRequest>
{
    protected override Task<OperationResult<DiscountedCartDTO>> HandleAsync(
        ApplyDiscountRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
