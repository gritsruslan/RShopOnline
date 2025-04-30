using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Configurations;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(i => new { i.ProductId, i.OrderId });
        
        builder
            .HasOne(i => i.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(i => i.OrderId);
        
        builder
            .HasOne(i => i.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(i => i.ProductId);
        
        builder.Property(i => i.PriceAtOrderTime).HasPrecision(18, 2);
    }
}