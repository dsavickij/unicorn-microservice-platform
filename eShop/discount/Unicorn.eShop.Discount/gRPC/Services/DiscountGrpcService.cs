using DiscountGrpcServiceProto;
using Grpc.Core;

namespace Unicorn.eShop.Discount.gRPC.Services;

public interface IDiscountGrpcService
{

}

public class DiscountGrpcService : DiscountGrpcServiceProto.DiscountGrpcServiceProto.DiscountGrpcServiceProtoBase
{
    public override Task<CartDiscountReply> GetCartDiscountAsync(CartDiscountRequest request, ServerCallContext context)
    {
        return Task.FromResult(new CartDiscountReply
        {
            DiscountId = Guid.NewGuid().ToString(),
            Description = "Test",
            DiscountCode = request.DiscountCode,
            DiscountPercentage = 20,
            Title = "Test titel"
        });
    }
}
