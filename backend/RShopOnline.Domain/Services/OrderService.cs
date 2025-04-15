using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class OrderService(
    IOrdersRepository ordersRepository, 
    IProductsRepository productsRepository,
    ICurrentUserService currentUserService) : IOrderService
{
    public async Task<Result<Order>> GetOrderById(Guid orderId, CancellationToken ct)
    {
        var order = await ordersRepository.GetOrderById(orderId, ct);
        if (order is null)
        {
            return new Error("Order not found!");
        }
        return order;
    }

    public async Task<IEnumerable<Order>> GetOrdersByCurrentUser(CancellationToken ct)
    {
        var userId = currentUserService.GetCurrentUserId() ??
                     throw new UnauthorizedAccessException("Unhandled unauthorized user exception!");

        return await ordersRepository.GetOrdersByUserId(userId, ct);
    }

    public async Task<Result<Order>> CreateOrder(CreateOrderCommand command, CancellationToken ct)
    {
        var userId = currentUserService.GetCurrentUserId() ??
                     throw new UnauthorizedAccessException("Unhandled unauthorized user exception!");

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
                return new Error($"Product {orderItemDto.ProductId} not found!");
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

    public async Task<EmptyResult> CancelOrder(Guid orderId, CancellationToken ct)
    {
        var userId = currentUserService.GetCurrentUserId() ??
                     throw new UnauthorizedAccessException("Unhandled unauthorized user exception!");
        
        var order = await ordersRepository.GetOrderById(orderId, ct);

        if (order is null)
        {
            return new Error($"Order with id {orderId} not found!");
        }

        if (order.UserId != userId)
        {
            //TODO Log unauthorized access
            return new Error("You are not allowed to cancel this order!");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return new Error($"Cannot cancel an order with status {order.Status.ToString()}!");
        }
        
        await ordersRepository.ChangeOrderStatus(orderId, OrderStatus.CanceledByUser, ct);
        return EmptyResult.Success();
    }

    public async Task<EmptyResult> UpdateOrderStatus(UpdateOrderStatusCommand command, CancellationToken ct)
    {
        var order = await ordersRepository.GetOrderById(command.OrderId, ct);
        
        if (order is null)
        {
            return new Error($"Order with id {command.OrderId} not found!");
        }

        await ordersRepository.ChangeOrderStatus(order.Id, command.NewOrderStatus, ct);
        return EmptyResult.Success();
    }
}