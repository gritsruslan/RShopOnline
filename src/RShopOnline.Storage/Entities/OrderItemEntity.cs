using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Entities;

public class OrderItemEntity
{
    public Guid OrderId { get; set; }
    
    public OrderEntity Order { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal PriceAtOrderTime { get; set; }
}