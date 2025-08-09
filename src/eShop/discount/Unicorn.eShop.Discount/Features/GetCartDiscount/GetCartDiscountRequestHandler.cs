using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Discount.SDK.DTOs;

namespace Unicorn.eShop.Discount.Features.GetCartDiscount;

public class GetCartDiscountRequestHandler : BaseHandler.WithResult<CartDiscount>.ForRequest<GetCartDiscountRequest>
{
    protected override Task<OperationResult<CartDiscount>> HandleAsync(GetCartDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = new CartDiscount
        {
            DiscountId = Guid.NewGuid(),
            Description = "Test",
            DiscountCode = request.DiscountCode,
            DiscountPercentage = 20,
            Title = "Test title"
        };

        return Task.FromResult(Ok(discount));
    }
}
