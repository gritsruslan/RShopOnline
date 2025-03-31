using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class CreateCategoryStorage(RShopDbContext dbContext, IMapper mapper) : ICreateCategoryStorage
{
    public async Task<Category> Create(string name, CancellationToken ct)
    {
        var category = new CategoryEntity { Name = name , Id = Guid.NewGuid() };
        await dbContext.Categories.AddAsync(category, ct);
        await dbContext.SaveChangesAsync(ct);
        return mapper.Map<Category>(category);
    }
}