using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.ParameterBindings;
using Unicorn.eShop.Discount.SDK;
using Unicorn.eShop.Discount.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Discount.SDK.Services.Http;

[UnicornHttpServiceMarker]
public interface IDiscountService
{
    [UnicornHttpGet("api/discounts/{discountCode}")]
    public Task<OperationResult<CartDiscount>> GetCartDiscountAsync([UnicornFromRoute] string discountCode);
}
