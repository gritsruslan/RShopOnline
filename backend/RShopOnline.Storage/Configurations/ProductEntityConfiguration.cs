using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Configurations;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        
        builder.Property(x => x.Description).HasMaxLength(1000);
        
        builder.Property(x => x.Price).HasPrecision(18, 2);
        
        builder.Property(x => x.Price);
        
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}