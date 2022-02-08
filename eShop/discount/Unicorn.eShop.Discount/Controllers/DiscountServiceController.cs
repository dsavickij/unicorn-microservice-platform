using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Discount.Features.GetCartDiscount;
using Unicorn.eShop.Discount.SDK.gRPC.Clients;
using Unicorn.eShop.Discount.SDK.Services.Http;

namespace Unicorn.eShop.Discount.Controllers;

public class DiscountServiceController : UnicornBaseController<IDiscountService>, IDiscountService
{
    private readonly ILogger<DiscountServiceController> _logger;

    public DiscountServiceController(ILogger<DiscountServiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/discounts/{discountCode}")]
    public async Task<OperationResult<CartDiscount>> GetCartDiscountAsync([FromRoute] string discountCode)
    {
        return await SendAsync(new GetCartDiscountRequest { DiscountCode = discountCode });
    }
}
