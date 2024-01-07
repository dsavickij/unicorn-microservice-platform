namespace Unicorn.eShop.Cart.SDK.DTOs;

public record CartDiscountDTO
{
    public Guid CartId { get; set; }
    public DiscountDTO Discount { get; set; } = new DiscountDTO();
    public TotalDTO Total { get; set; } = new TotalDTO();

}

public record TotalDTO
{
    public decimal OriginalTotal { get; set; }
    public decimal DiscountedTotal { get; set; }
}

public record DiscountDTO
{
    public Guid CartDiscountId { get; set; }
    public decimal Discount { get; set; }
    public double DiscountPercentage { get; set; }
    public string Message { get; set; } = string.Empty;
}
