using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface IOrderService
{
    Task<Result<Order>> GetOrderById(Guid orderId, CancellationToken ct);
    
    Task<IEnumerable<Order>> GetOrdersByCurrentUser(CancellationToken ct);
        
    Task<Result<Order>> CreateOrder(CreateOrderCommand command, CancellationToken ct);
    
    Task<EmptyResult> CancelOrder(Guid orderId, CancellationToken ct);
    
    Task<EmptyResult> UpdateOrderStatus(UpdateOrderStatusCommand command, CancellationToken ct);
}