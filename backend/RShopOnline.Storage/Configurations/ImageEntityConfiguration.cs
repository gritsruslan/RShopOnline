using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Configurations;

public class ImageEntityConfiguration : IEntityTypeConfiguration<ImageInfoEntity>
{
    public void Configure(EntityTypeBuilder<ImageInfoEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Id);
        
        builder.Property(i => i.Extension).HasMaxLength(4);
        
        builder
            .HasOne<ProductEntity>(u => u.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(x => x.ProductId);
    }
}