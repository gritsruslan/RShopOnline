using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Commands;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus NewOrderStatus);