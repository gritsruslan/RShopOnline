using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetProducts;

public class GetAllProductsUseCase(IGetProductsStorage storage) : IGetAllProductsUseCase
{
    public async Task<IEnumerable<Product>> Handle( CancellationToken ct)
    {
        return await storage.GetAllProducts(ct);
    }
}