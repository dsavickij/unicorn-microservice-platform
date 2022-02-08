using DiscountGrpcServiceProto;
using Grpc.Core;
using MediatR;
using Unicorn.eShop.Discount.Features.GetCartDiscount;

namespace Unicorn.eShop.Discount.gRPC.Services;

public interface IDiscountGrpcService
{

}

public class DiscountGrpcService : DiscountGrpcServiceProto.DiscountGrpcServiceProto.DiscountGrpcServiceProtoBase
{
    private readonly IMediator _mediator;

    public DiscountGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<CartDiscountReply> GetCartDiscountAsync(CartDiscountRequest request, ServerCallContext context)
    {
        var mediatorReq = new GetCartDiscountRequest { DiscountCode = request.DiscountCode };
        var result = await _mediator.Send(mediatorReq);

        if (result.IsSuccess)
        {
            return new CartDiscountReply
            {
                DiscountId = result.Data!.DiscountId.ToString(),
                Description = result.Data!.Description,
                DiscountCode = result.Data!.DiscountCode,
                DiscountPercentage = result.Data!.DiscountPercentage,
                Title = result.Data!.Title
            };
        }

        throw new RpcException(new Status(StatusCode.Internal, string.Join("; ", result.Errors.Select(x => x.Message))));
    }
}
