namespace RShopAPI_Test.Services.Commands;

public record CreateProductCommand(string Name, decimal Price, Guid CategoryId, bool InStock, string Description);