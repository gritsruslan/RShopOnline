using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface ICreateProductStorage
{
    Task<Product> CreateProduct(string name, decimal price, Guid categoryId, bool inStock, string description, CancellationToken ct);
}