namespace Unicorn.eShop.CartService.Controllers;

public record CartDiscountDTO
{
    public Guid DiscountId { get; set; }
    public string DiscountTitle { get; set; } = "No discount";
    public string DiscountCode { get; set; } = string.Empty;
    public double DiscountPercentage { get; set; } = 0;
}