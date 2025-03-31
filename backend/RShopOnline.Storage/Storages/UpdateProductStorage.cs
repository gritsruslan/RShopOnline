using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class UpdateProductStorage(RShopDbContext dbContext, IMapper mapper) : IUpdateProductStorage
{
    public async Task<Product> UpdateProduct(Guid id, string name, decimal price, bool inStock, string description, CancellationToken ct)
    {
        await dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(p => p.Name, name)
                .SetProperty(p => p.Price, price)
                .SetProperty(p => p.InStock, inStock)
                .SetProperty(p => p.Description, description), ct);
        
        await dbContext.SaveChangesAsync(ct);
        
        return await dbContext.Products
            .Where(p => p.Id == id)
            .Select(p => mapper.Map<Product>(p))
            .FirstAsync(ct);
    }
}