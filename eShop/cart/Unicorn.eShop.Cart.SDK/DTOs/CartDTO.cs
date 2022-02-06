namespace Unicorn.eShop.Cart.SDK.DTOs;

public record CartDTO
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<CartItemDTO> Items { get; set; } = Enumerable.Empty<CartItemDTO>();
    public decimal TotalPrice { get; set; }
}