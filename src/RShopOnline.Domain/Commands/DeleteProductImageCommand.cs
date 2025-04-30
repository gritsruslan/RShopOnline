namespace RShopAPI_Test.Services.Commands;

public record DeleteProductImageCommand(Guid ProductId, string ImageName);