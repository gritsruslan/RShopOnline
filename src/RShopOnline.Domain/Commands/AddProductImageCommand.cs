using Microsoft.AspNetCore.Http;

namespace RShopAPI_Test.Services.Commands;

public record AddProductImageCommand(IFormFile Image, Guid ProductId);