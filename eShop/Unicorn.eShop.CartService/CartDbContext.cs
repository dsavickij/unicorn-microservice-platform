using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Unicorn.eShop.CartService.Entities;

namespace Unicorn.eShop.CartService;

public class CartDbContext : DbContext
{
    private readonly CartHostSettings _settings;

    public DbSet<Cart> Carts { get; set; }
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
