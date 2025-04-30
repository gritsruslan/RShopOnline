using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Repositories;

public class OrdersRepository(RShopDbContext dbContext, IMapper mapper) : IOrdersRepository
{
    public async Task<Order> CreateOrder(Guid orderId, Guid userId, IEnumerable<OrderItem> orderItems, CancellationToken ct)
    {
        var orderItemEntities = orderItems
            .Select(mapper.Map<OrderItemEntity>).ToList();
        
        var orderEntity = new OrderEntity()
        {
            Id = orderId,
            UserId = userId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            OrderItems = orderItemEntities
        };

        await dbContext.Orders.AddAsync(orderEntity, ct);
        await dbContext.SaveChangesAsync(ct);
        return mapper.Map<Order>(orderEntity);
    }

    public async Task<Order?> GetOrderById(Guid id, CancellationToken ct)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Include(o => o.OrderItems)
            .ProjectTo<Order>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid userId, CancellationToken ct)
    {
         var orders = await dbContext.Orders.AsNoTracking()
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ProjectTo<Order>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
         return orders;
    }

    public async Task ChangeOrderStatus(Guid id, OrderStatus newStatus, CancellationToken ct)
    {
        await dbContext.Orders
            .Where(o => o.Id == id)
            .ExecuteUpdateAsync(o => 
                o.SetProperty(o => o.Status, newStatus), ct);
        
        await dbContext.SaveChangesAsync(ct);
    }
}