namespace RShopAPI_Test.DTOs;

public record UpdateProductRequest(Guid Id, string Name, decimal Price, Guid CategoryId, bool InStock, string Description);