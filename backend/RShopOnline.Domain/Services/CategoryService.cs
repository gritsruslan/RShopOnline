using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class CategoryService(ICategoriesRepository repository) : ICategoryService
{
    public async Task CreateCategory(string name, CancellationToken ct)
    {
        await repository.CreateCategory(name, ct);
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken ct)
    {
        return await repository.GetCategories(ct);
    }
}