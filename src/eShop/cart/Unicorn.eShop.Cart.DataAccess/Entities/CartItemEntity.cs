using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Unicorn.eShop.Cart.DataAccess.Entities;

[Table("CartItems")]
[Index(nameof(CartId), nameof(CatalogItemId), IsUnique = true)]
public record CartItemEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid CatalogItemId { get; set; }
    [Required]
    public Guid CartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsAvailable { get; set; }
}
