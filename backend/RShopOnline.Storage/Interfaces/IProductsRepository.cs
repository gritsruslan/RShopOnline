using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IProductsRepository
{
    Task<Product> CreateProduct(
        string name,
        decimal price,
        Guid categoryId,
        bool inStock,
        string description,
        CancellationToken ct);
    
    Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct);
    
    
    Task<IEnumerable<Product>> GetProducts(
        Guid categoryId, 
        int skip, int take, 
        string orderByField, 
        bool ascending, 
        CancellationToken ct);
    
    Task<Product?> GetProductById(Guid id, CancellationToken ct);
    
    Task<bool> ProductExists(Guid id, CancellationToken ct);
    
    Task<Product> UpdateProduct(
        Guid id,
        string name,
        decimal price, 
        bool inStock, 
        string description, 
        CancellationToken ct);
}