using Microsoft.EntityFrameworkCore;
using Unicorn.eShop.Cart.Entities;

namespace Unicorn.eShop.Cart;

public class CartDbContext : DbContext
{
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }

    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }
}
