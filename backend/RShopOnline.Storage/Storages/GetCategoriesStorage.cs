using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetCategoriesStorage(RShopDbContext dbContext) : IGetCategoriesStorage
{
    public async Task<IEnumerable<Category>> GetCategories(CancellationToken ct)
    {
        return await dbContext.Categories
            .Select(c => new Category {Id = c.Id, Name = c.Name })
            .ToListAsync(ct);
    }
}