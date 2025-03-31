using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetCategoriesStorage(RShopDbContext dbContext, IMapper mapper) : IGetCategoriesStorage
{
    public async Task<IEnumerable<Category>> GetCategories(CancellationToken ct)
    {
        return await dbContext.Categories
            .AsNoTracking()
            .Select(c => mapper.Map<Category>(c))
            .ToListAsync(ct);
    }

    public async Task<Category?> GetCategoryById(Guid id, CancellationToken ct)
    {
        var categoryEntity = await 
            dbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        
        return categoryEntity is null ? null : mapper.Map<Category>(categoryEntity);
    }

    public async Task<bool> DoesCategoryExist(Guid id, CancellationToken ct)
    {
        return await GetCategoryById(id, ct) != null;
    }
}