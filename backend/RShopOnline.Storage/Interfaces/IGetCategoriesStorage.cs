using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IGetCategoriesStorage
{
    Task<IEnumerable<Category>> GetCategories(CancellationToken ct);
    
    Task<Category?> GetCategoryById(Guid id, CancellationToken ct);
    
    Task<bool> DoesCategoryExist(Guid id, CancellationToken ct);
}