namespace RShopAPI_Test.DTOs;

public record CreateProductRequest(string Name, decimal Price, Guid CategoryId, bool InStock, string Description);