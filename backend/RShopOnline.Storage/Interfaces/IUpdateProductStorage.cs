using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IUpdateProductStorage
{
    Task<Product> UpdateProduct(Guid id, string name, decimal price, bool inStock, string description, CancellationToken ct);
}