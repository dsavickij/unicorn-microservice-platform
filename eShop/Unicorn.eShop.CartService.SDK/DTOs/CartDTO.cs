using Unicorn.eShop.CartService.SDK.DTOs;

namespace Unicorn.eShop.CartService.Controllers;

public record CartDTO
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<CartItemDTO> Items { get; set; } = Enumerable.Empty<CartItemDTO>();
    public CartDiscountDTO CartDiscount { get; set; } = new CartDiscountDTO();
    public decimal TotalPrice { get; set; }
}