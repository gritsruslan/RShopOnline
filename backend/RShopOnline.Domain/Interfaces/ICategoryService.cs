using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Interfaces;

public interface ICategoryService
{
    Task CreateCategory(string name, CancellationToken ct);
    
    Task<IEnumerable<Category>> GetCategories(CancellationToken ct);
}