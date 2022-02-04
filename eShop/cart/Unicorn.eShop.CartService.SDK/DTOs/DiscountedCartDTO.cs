using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.eShop.CartService.Controllers;

namespace Unicorn.eShop.CartService.SDK.DTOs;

public record DiscountedCartDTO
{
    public CartDTO OriginalCart { get; set; } = new CartDTO();
    public CartDiscountDTO Discount { get; set; } = new CartDiscountDTO();
    public decimal DiscountedTotalPrice { get; set; } = 0;
}
