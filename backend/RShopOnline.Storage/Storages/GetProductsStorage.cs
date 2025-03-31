using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetProductsStorage(RShopDbContext dbContext, IMapper mapper) : IGetProductsStorage
{
    public async Task<Product?> GetProductById(Guid id, CancellationToken ct)
    {
        var productEntity = await dbContext.Products
            .AsNoTracking().
            FirstOrDefaultAsync(p => p.Id == id, ct); 
        return productEntity is null ? null : mapper.Map<Product>(productEntity);
    }
}