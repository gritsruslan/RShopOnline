using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IGetProductsStorage
{
    Task<Product?> GetProductById(Guid id, CancellationToken ct);
}