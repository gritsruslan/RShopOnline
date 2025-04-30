using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IOrdersRepository
{
    Task<Order> CreateOrder(Guid orderId, Guid userId, IEnumerable<OrderItem> orderItems, CancellationToken ct);
    
    Task<Order?> GetOrderById(Guid id, CancellationToken ct);
    
    Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId, CancellationToken ct);
    
    Task ChangeOrderStatus(Guid id, OrderStatus newStatus, CancellationToken ct);
}