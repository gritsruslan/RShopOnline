using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetProducts;

public class GetProductsUseCase(IGetProductsStorage storage) : IGetProductsUseCase
{
    public async Task<IEnumerable<Product>> Handle( CancellationToken ct)
    {
        return await storage.GetProducts(ct);
    }
}