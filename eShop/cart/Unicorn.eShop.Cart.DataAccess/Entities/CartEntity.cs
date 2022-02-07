using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unicorn.eShop.Cart.Entities;

[Table("Carts")]
[Index(nameof(UserId), IsUnique = true)]
[Index(nameof(Id), IsUnique = true)]
[Index(nameof(Id), nameof(UserId), IsUnique = true)]
public record CartEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<CartItemEntity> Items { get; set; } = Enumerable.Empty<CartItemEntity>();
    public decimal TotalPrice => Items.Sum(x => x.UnitPrice * x.Quantity);
}
