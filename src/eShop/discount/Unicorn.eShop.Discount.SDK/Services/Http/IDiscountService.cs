using Refit;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;
using Unicorn.eShop.Discount.SDK;
using Unicorn.eShop.Discount.SDK.DTOs;

[assembly: UnicornServiceHostName(Constants.ServiceHostName)]

namespace Unicorn.eShop.Discount.SDK.Services.Http;

[UnicornRestServiceMarker]
public interface IDiscountService
{
    [Get("api/discounts/{discountCode}")]
    public Task<OperationResult<CartDiscount>> GetCartDiscountAsync(string discountCode);
}
