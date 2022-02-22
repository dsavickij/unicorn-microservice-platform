namespace Unicorn.eShop.Discount.SDK.DTOs;

public record CartDiscount
{
    public Guid DiscountId { get; set; }
    public string Title { get; set; } = "No discount";
    public string Description { get; set; } = string.Empty;
    public string DiscountCode { get; set; } = string.Empty;
    public double DiscountPercentage { get; set; }
}
