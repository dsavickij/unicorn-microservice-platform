using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Unicorn.eShop.Cart.Entities;

namespace Unicorn.eShop.Cart;

public class CartDbContext : DbContext
{
    private readonly CartHostSettings _settings;

    public DbSet<Entities.Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public CartDbContext(IOptions<CartHostSettings> settings)
    {
        _settings = settings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_settings.DbConnectionString);
    }
}
