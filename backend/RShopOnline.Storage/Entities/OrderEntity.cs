using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Storage.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public UserEntity User { get; set; } = null!;
    
    public OrderStatus Status { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = null!;
}