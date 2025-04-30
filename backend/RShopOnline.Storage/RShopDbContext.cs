using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Storage.Configurations;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage;

public class RShopDbContext(DbContextOptions<RShopDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<CategoryEntity> Categories { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }
    
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    
    public DbSet<ImageInfoEntity> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}