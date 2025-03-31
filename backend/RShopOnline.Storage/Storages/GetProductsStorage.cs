using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetProductsStorage(RShopDbContext dbContext, IMapper mapper) : IGetProductsStorage
{
    public async Task<IEnumerable<Product>> GetProducts(CancellationToken ct)
    {
       return await dbContext.Products.AsNoTracking().Select(p => mapper.Map<Product>(p)).ToListAsync(ct);
    }

    public async Task<Product?> GetProductById(Guid id, CancellationToken ct)
    {
        var productEntity = await dbContext.Products
            .AsNoTracking().
            FirstOrDefaultAsync(p => p.Id == id, ct); 
        return productEntity is null ? null : mapper.Map<Product>(productEntity);
    }

    public async Task<bool> DoesProductExist(Guid id, CancellationToken ct)
    {
        return await GetProductById(id, ct) != null;
    }
}