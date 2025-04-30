using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface IOrderService
{
    Task<Result<Order>> GetOrderById(GetOrderByIdCommand command, CancellationToken ct);
    
    Task<Result<IEnumerable<Order>>> GetOrdersByCurrentUser(GetOrdersByCurrentUserCommand command, CancellationToken ct);
        
    Task<Result<Order>> CreateOrder(CreateOrderCommand command, CancellationToken ct);
    
    Task<EmptyResult> CancelOrder(CancelOrderCommand command, CancellationToken ct);
    
    Task<EmptyResult> UpdateOrderStatus(UpdateOrderStatusCommand command, CancellationToken ct);
}