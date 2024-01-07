using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public class ServiceDiscoveryDbContext : DbContext
{
    public DbSet<ServiceHostEntity> ServiceHosts{ get; set; }

    public DbSet<HttpServiceConfigurationEntity> HttpServiceConfigurations { get; set; }

    public DbSet<GrpcServiceConfigurationEntity> GrpcServiceConfigurations { get; set; }

    public ServiceDiscoveryDbContext(DbContextOptions<ServiceDiscoveryDbContext> options) : base(options)
    {      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceHostEntity>()
            .ToTable("ServiceHosts")
            .HasKey(x => x.Name);

        modelBuilder.Entity<HttpServiceConfigurationEntity>()
             .ToTable("HttpServiceConfigurations")
             .HasKey(x => x.Id);

        modelBuilder.Entity<HttpServiceConfigurationEntity>()
            .HasIndex(x => x.ServiceHostName)
            .IsUnique();

        modelBuilder.Entity<HttpServiceConfigurationEntity>()
            .HasOne(x => x.ServiceHost)
            .WithOne()
            .HasForeignKey<HttpServiceConfigurationEntity>(x => x.ServiceHostName)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GrpcServiceConfigurationEntity>()
            .ToTable("GrpcServiceConfigurations")
            .HasKey(x => x.Id);

        modelBuilder.Entity<GrpcServiceConfigurationEntity>()
            .HasIndex(x => x.ServiceHostName)
            .IsUnique();

        modelBuilder.Entity<GrpcServiceConfigurationEntity>()
            .HasOne(x => x.ServiceHost)
            .WithOne()
            .HasForeignKey<GrpcServiceConfigurationEntity>(x => x.ServiceHostName)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
