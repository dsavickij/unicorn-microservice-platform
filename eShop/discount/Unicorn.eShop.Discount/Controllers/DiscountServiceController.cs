using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK;
using Unicorn.eShop.Discount.SDK.Services.Http;

namespace Unicorn.eShop.Discount.Controllers;

public class DiscountServiceController : UnicornBaseController<IDiscountService>, IDiscountService
{
    private readonly ILogger<DiscountServiceController> _logger;

    public DiscountServiceController(ILogger<DiscountServiceController> logger)
    {
        _logger = logger;
    }
}
