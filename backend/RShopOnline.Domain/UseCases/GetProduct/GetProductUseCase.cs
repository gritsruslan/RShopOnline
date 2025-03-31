using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetProduct;

public class GetProductUseCase(IGetProductsStorage storage) : IGetProductUseCase
{
    public async Task<Result<Product>> Handle(Guid id, CancellationToken ct)
    {
        var product = await storage.GetProductById(id, ct);
        if (product == null)
        {
            return new Error("Product not found");
        }
        return product;
    }
}