using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RShopAPI_Test.Storage.Entities;

namespace RShopAPI_Test.Storage.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        
        builder.Property(x => x.Email).HasMaxLength(320);
        
        builder.Property(x => x.Role).HasConversion<int>();
        
        builder.HasMany<OrderEntity>(x => x.Orders)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}