using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IGetProductsStorage
{
    Task<IEnumerable<Product>> GetProducts(CancellationToken ct);
    
    Task<Product?> GetProductById(Guid id, CancellationToken ct);
    
    Task<bool> DoesProductExist(Guid id, CancellationToken ct);
}