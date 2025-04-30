namespace RShopAPI_Test.DTOs;

public record CreateOrderRequest(List<OrderItemServiceDto> OrderItems);

public record OrderItemServiceDto(Guid ProductId, int Quantity);