using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Discount.SDK.gRPC.Clients;

namespace Unicorn.eShop.Discount.Features.GetCartDiscount;

public class GetCartDiscountRequestHandler : BaseHandler.WithResult<CartDiscount>.For<GetCartDiscountRequest>
{
    protected override Task<OperationResult<CartDiscount>> HandleAsync(GetCartDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = new CartDiscount {  
            DiscountId = Guid.NewGuid(),
            Description = "Test",
            DiscountCode = request.DiscountCode,
            DiscountPercentage = 20,
            Title = "Test title"
        };

        return Task.FromResult(Ok(discount));
    }
}
