using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;
using Unicorn.eShop.CartService.SDK.DTOs;
using Unicorn.eShop.Discount.SDK.gRPC.Clients;

namespace Unicorn.eShop.CartService.Features.ApplyDiscount;

public class ApplyDiscountRequestHandler : BaseHandler.WithResult<DiscountedCartDTO>.For<ApplyDiscountRequest>
{
    private readonly IDiscountGrpcServiceClient? _discountClient;

    public ApplyDiscountRequestHandler(IServiceProvider discountGrpServiceClient)
    {
        _discountClient = discountGrpServiceClient.GetService<IDiscountGrpcServiceClient>();
    }

    protected override async Task<OperationResult<DiscountedCartDTO>> HandleAsync(
        ApplyDiscountRequest request, CancellationToken cancellationToken)
    {
        var result = await _discountClient.GetCartDiscountAsync(request.DiscountCode);

        return Ok(new DiscountedCartDTO());
    }
}
