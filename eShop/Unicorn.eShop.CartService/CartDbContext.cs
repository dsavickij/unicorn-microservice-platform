using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unicorn.eShop.CartService;

public class CartDbContext : DbContext
{
    private readonly CartHostSettings _settings;

    public DbSet<Cart> Carts { get; set; }

    public CartDbContext(IOptions<CartHostSettings> settings)
    {
        _settings = settings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_settings.DbConnectionString);
    }
}

[Table("Carts")]
public record Cart
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "sdsd";
}
