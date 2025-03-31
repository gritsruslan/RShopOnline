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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}