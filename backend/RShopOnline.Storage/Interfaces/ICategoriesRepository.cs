using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface ICategoriesRepository
{
    Task CreateCategory(string name, CancellationToken ct);
    
    Task<IEnumerable<Category>> GetCategories(CancellationToken ct);
    
    Task<Category?> GetCategoryById(Guid id, CancellationToken ct);
    
    Task<bool> CategoryExists(Guid id, CancellationToken ct);
}