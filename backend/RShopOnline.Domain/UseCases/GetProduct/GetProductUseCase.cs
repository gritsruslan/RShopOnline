using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetProduct;

public class GetProductUseCase(IGetProductsStorage storage) : IGetProductUseCase
{
    public async Task<Product?> Handle(Guid id, CancellationToken ct)
    {
        return await storage.GetProductById(id, ct);
    }
}