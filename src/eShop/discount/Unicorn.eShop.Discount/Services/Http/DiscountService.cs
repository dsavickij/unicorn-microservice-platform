using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Discount.Features.GetCartDiscount;
using Unicorn.eShop.Discount.SDK.DTOs;
using Unicorn.eShop.Discount.SDK.Services.Http;

namespace Unicorn.eShop.Discount.Services.Http;

public class DiscountService : UnicornHttpService<IDiscountService>, IDiscountService
{
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(ILogger<DiscountService> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/discounts/{discountCode}")]
    public async Task<OperationResult<CartDiscount>> GetCartDiscountAsync([FromRoute] string discountCode)
    {
        return await SendAsync(new GetCartDiscountRequest { DiscountCode = discountCode });
    }
}
