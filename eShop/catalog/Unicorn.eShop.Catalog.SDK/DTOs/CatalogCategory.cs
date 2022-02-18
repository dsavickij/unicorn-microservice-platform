namespace Unicorn.eShop.Catalog.SDK.Services.Http;

public record CatalogCategory
{
    public Guid Id { get; set; }
    public string DisplayTitle { get; set; } = "No display title";
    public string Title { get; set; } = "No title";
    public string Description { get; set; } = "No description";
}
