using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Core.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }

    public required ICollection<OrderItem> OrderItems { get; set; }
    
    public decimal TotalPrice => OrderItems.Sum(o => o.PriceAtOrderTime * o.Quantity);
}