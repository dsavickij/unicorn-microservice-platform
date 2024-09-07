using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.Cart.SDK.DTOs;
using Unicorn.eShop.Discount.SDK.Services.gRPC.Clients;

namespace Unicorn.eShop.Cart.Features.ApplyDiscount;

public class ApplyDiscountRequestHandler : BaseHandler.WithResult<DiscountedCartDTO>.ForRequest<ApplyDiscountRequest>
{
    private readonly IDiscountGrpcServiceClient _discountClient;

    public ApplyDiscountRequestHandler(IDiscountGrpcServiceClient discountGrpServiceClient)
    {
        _discountClient = discountGrpServiceClient;
    }

    protected override async Task<OperationResult<DiscountedCartDTO>> HandleAsync(
        ApplyDiscountRequest request, CancellationToken cancellationToken)
    {
        var result = await _discountClient.GetCartDiscountAsync(request.DiscountCode);

        return Ok(new DiscountedCartDTO());
    }
}
