using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.Discount.SDK.gRPC.Clients;

[assembly: UnicornServiceHostName("Unicorn.eShop.Discount")]

namespace Unicorn.eShop.Discount.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IDiscountService
{
    [UnicornHttpGet("api/discounts/{discountCode}")]
    public Task<OperationResult<CartDiscount>> GetCartDiscountAsync([UnicornFromRoute] string discountCode);
}
