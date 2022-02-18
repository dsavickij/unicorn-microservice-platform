using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.eShop.Cart.SDK.DTOs;

public record CartItemDTO
{
    public Guid CatalogItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsAvailable { get; set; }
}
