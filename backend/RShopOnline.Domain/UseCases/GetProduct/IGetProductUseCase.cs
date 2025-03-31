using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.GetProduct;

public interface IGetProductUseCase
{
    Task<Product?> Handle(Guid id, CancellationToken ct);
}