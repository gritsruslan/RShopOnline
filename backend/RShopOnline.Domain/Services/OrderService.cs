using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class OrderService(
    IOrdersRepository ordersRepository, 
    IProductsRepository productsRepository,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IOrderService
{
    public async Task<Result<Order>> GetOrderById(GetOrderByIdCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<GetOrderByIdCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        var order = await ordersRepository.GetOrderById(command.OrderId, ct);
        if (order is null)
        {
            return new Error("Order not found!", ErrorCode.NotFound);
        }
        return order;
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersByCurrentUser(
        GetOrdersByCurrentUserCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<GetOrdersByCurrentUserCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        var userId = identityProvider.Current.Id;
        var orders = await ordersRepository.GetOrdersByUserId(userId, ct);
        return Result<IEnumerable<Order>>.Success(orders);
    }

    public async Task<Result<Order>> CreateOrder(CreateOrderCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<CreateOrderCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        var userId = identityProvider.Current.Id;
        var orderId = Guid.NewGuid();
        var processedOrderItems = new List<OrderItem>(command.OrderItems.Count);

        if (command.OrderItems.Count == 0)
        {
            return new Error("Order is empty!");
        }
        
        foreach (var orderItemDto in command.OrderItems)
        {
            if (orderItemDto.Quantity <= 0)
            {
                return new Error($"Invalid order quantity for product {orderItemDto.ProductId}");
            }
            
            var product = await productsRepository.GetProductById(orderItemDto.ProductId, ct);
            if (product is null)
            {
                return new Error($"Product {orderItemDto.ProductId} not found!", ErrorCode.NotFound);
            }

            if (!product.InStock)
            {
                return new Error($"Product {product.Id} is out of stock!");
            }

            if (processedOrderItems.Any(p => p.ProductId == orderItemDto.ProductId))
            {
                return new Error($"Product {orderItemDto.ProductId} added to order multiple times!");
            }

            var orderItem = new OrderItem()
            {
                OrderId = orderId,
                ProductId = product.Id,
                Quantity = orderItemDto.Quantity,
                PriceAtOrderTime = product.Price
            };
            
            processedOrderItems.Add(orderItem);
        }

        return await ordersRepository.CreateOrder(orderId, userId, processedOrderItems, ct);
    }

    public async Task<EmptyResult> CancelOrder(CancelOrderCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<CancelOrderCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        var userId = identityProvider.Current.Id;
        var order = await ordersRepository.GetOrderById(command.OrderId, ct);

        if (order is null)
        {
            return new Error($"Order with id {command.OrderId} not found!", ErrorCode.NotFound);
        }

        if (order.UserId != userId)
        {
            return new Error("You are not allowed to cancel this order!");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return new Error($"Cannot cancel an order with status {order.Status.ToString()}!");
        }
        
        await ordersRepository.ChangeOrderStatus(command.OrderId, OrderStatus.CanceledByUser, ct);
        return EmptyResult.Success();
    }

    public async Task<EmptyResult> UpdateOrderStatus(UpdateOrderStatusCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<UpdateOrderStatusCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        var order = await ordersRepository.GetOrderById(command.OrderId, ct);
        if (order is null)
        {
            return new Error($"Order with id {command.OrderId} not found!", ErrorCode.NotFound);
        }

        await ordersRepository.ChangeOrderStatus(order.Id, command.NewOrderStatus, ct);
        return EmptyResult.Success();
    }
}