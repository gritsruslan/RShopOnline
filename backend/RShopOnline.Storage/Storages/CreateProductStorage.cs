using AutoMapper;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class CreateProductStorage(RShopDbContext dbContext, IMapper mapper) : ICreateProductStorage
{
    public async Task<Product> CreateProduct(
        string name,
        decimal price, 
        Guid categoryId, 
        bool inStock, 
        string description, 
        CancellationToken ct)
    {
        var product = new ProductEntity() 
            {Id = Guid.NewGuid(), Name = name, Price = price, CategoryId = categoryId, InStock = inStock, Description = description };
        await dbContext.Products.AddAsync(product, ct);
        await dbContext.SaveChangesAsync(ct);
        return mapper.Map<Product>(product);
    }
}