namespace RShopAPI_Test.Services.UseCases.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, decimal Price, Guid CategoryId, bool InStock, string Description);