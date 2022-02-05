using Microsoft.EntityFrameworkCore;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public class ServiceDiscoveryDbContext : DbContext
{
    private readonly DbContextOptions<ServiceDiscoveryDbContext> _options;

    public DbSet<HttpServiceConfigurationEntity> HttpServiceConfigurations { get; set; }
    public DbSet<GrpcServiceConfigurationEntity> GrpcServiceConfigurations { get; set; }

    public ServiceDiscoveryDbContext(DbContextOptions<ServiceDiscoveryDbContext> options)
    {
        _options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<HttpServiceConfigurationEntity>()
            .HasKey(x => x.Name);

        modelBuilder.Entity<GrpcServiceConfigurationEntity>()
            .HasKey(x => x.Name);
    }
}
