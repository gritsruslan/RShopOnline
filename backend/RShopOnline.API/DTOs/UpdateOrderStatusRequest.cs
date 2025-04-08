using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.DTOs;

public record UpdateOrderStatusRequest(Guid OrderId, OrderStatus OrderStatus);