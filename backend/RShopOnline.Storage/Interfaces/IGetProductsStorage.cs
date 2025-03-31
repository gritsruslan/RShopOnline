using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IGetProductsStorage
{
    Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct);
    
    Task<IEnumerable<Product>> GetProducts(
        Guid categoryId, 
        int skip, 
        int take, 
        string orderByField, 
        bool ascending,  
        CancellationToken ct);
    
    Task<Product?> GetProductById(Guid id, CancellationToken ct);
    
    Task<bool> DoesProductExist(Guid id, CancellationToken ct);
}