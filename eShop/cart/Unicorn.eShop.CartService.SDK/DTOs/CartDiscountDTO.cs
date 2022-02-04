namespace Unicorn.eShop.CartService.Controllers;

public record CartDiscountDTO
{
    public Guid CartId { get; set; }
    public DiscountDTO Discount { get; set; } = new DiscountDTO();
    public TotalDTO Total { get; set; } = new TotalDTO();

}

public record TotalDTO
{
    public decimal OriginalTotal { get; set; } = 0;
    public decimal DiscountedTotal { get; set; } = 0;
}

public record DiscountDTO
{
    public Guid CartDiscountId { get; set; }
    public decimal Discount { get; set; } = 0;
    public double DiscountPercentage { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
}