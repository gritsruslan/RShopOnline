namespace RShopAPI_Test.Services.Commands;

public record CreateOrderCommand(List<OrderItemDto> OrderItems);

public record OrderItemDto(Guid ProductId, int Quantity);