using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Core.Models;

public class Order
{
    public Guid Id { get; set; }
    
    public OrderStatus Status { get; set; }
}