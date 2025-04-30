namespace RShopAPI_Test.DTOs;

public record AddProductImageRequest(IFormFile Photo, Guid ProductId);