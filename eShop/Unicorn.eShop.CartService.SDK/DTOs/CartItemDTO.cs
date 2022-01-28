﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.eShop.CartService.SDK.DTOs;

public record CartItemDTO
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }    
}