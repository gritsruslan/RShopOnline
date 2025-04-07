using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Repositories;

public class CategoriesRepository(RShopDbContext dbContext, IMapper mapper) : ICategoriesRepository
{
    public async Task CreateCategory(string name, CancellationToken ct)
    {
        await dbContext.Categories.AddAsync(new CategoryEntity { Id = Guid.NewGuid(), Name = name }, ct);
        await dbContext.SaveChangesAsync(ct);
        
    }

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

    public async Task<bool> CategoryExists(Guid id, CancellationToken ct)
    {
        return await GetCategoryById(id, ct) != null;
    }
}