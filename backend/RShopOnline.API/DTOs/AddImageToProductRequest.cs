namespace RShopAPI_Test.DTOs;

public record AddImageToProductRequest(IFormFile Photo, Guid ProductId);