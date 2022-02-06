﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unicorn.Core.Services.ServiceDiscovery.DataAccess;

#nullable disable

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess.Migrations
{
    [DbContext(typeof(ServiceDiscoveryDbContext))]
    partial class ServiceDiscoveryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Unicorn.Core.Services.ServiceDiscovery.DataAccess.GrpcServiceConfigurationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("ServiceHostName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceHostName")
                        .IsUnique();

                    b.ToTable("GrpcServiceConfigurations", (string)null);
                });

            modelBuilder.Entity("Unicorn.Core.Services.ServiceDiscovery.DataAccess.HttpServiceConfigurationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceHostName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceHostName")
                        .IsUnique();

                    b.ToTable("HttpServiceConfigurations", (string)null);
                });

            modelBuilder.Entity("Unicorn.Core.Services.ServiceDiscovery.DataAccess.ServiceHost", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("ServiceHosts", (string)null);
                });

            modelBuilder.Entity("Unicorn.Core.Services.ServiceDiscovery.DataAccess.GrpcServiceConfigurationEntity", b =>
                {
                    b.HasOne("Unicorn.Core.Services.ServiceDiscovery.DataAccess.ServiceHost", "ServiceHost")
                        .WithOne()
                        .HasForeignKey("Unicorn.Core.Services.ServiceDiscovery.DataAccess.GrpcServiceConfigurationEntity", "ServiceHostName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceHost");
                });

            modelBuilder.Entity("Unicorn.Core.Services.ServiceDiscovery.DataAccess.HttpServiceConfigurationEntity", b =>
                {
                    b.HasOne("Unicorn.Core.Services.ServiceDiscovery.DataAccess.ServiceHost", "ServiceHost")
                        .WithOne()
                        .HasForeignKey("Unicorn.Core.Services.ServiceDiscovery.DataAccess.HttpServiceConfigurationEntity", "ServiceHostName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceHost");
                });
#pragma warning restore 612, 618
        }
    }
}
