using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface ICreateCategoryStorage
{
    public Task<Category> Create(string name, CancellationToken ct);
}