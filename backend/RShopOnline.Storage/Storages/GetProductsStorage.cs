using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetProductsStorage(RShopDbContext dbContext, IMapper mapper) : IGetProductsStorage
{
    public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct)
    {
       return await dbContext.Products.AsNoTracking().Select(p => mapper.Map<Product>(p)).ToListAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetProducts(
        Guid categoryId, 
        int skip, int take, 
        string orderByField, 
        OrderByDirection orderByDirection, 
        CancellationToken ct)
    {
        var query = dbContext.Products.AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .Skip(skip)
            .Take(take);

        Expression<Func<ProductEntity, object>> orderByExpression = orderByField switch
        {
            "Name" => p => p.Name,
            "Price" => p => p.Price,
            "InStock" => p => p.InStock,
            _ => throw new Exception("Invalid order field!")
        };

        if (orderByDirection == OrderByDirection.Ascending)
            query = query.OrderBy(orderByExpression);
        else
            query = query.OrderByDescending(orderByExpression);

        return await query.Select(p => mapper.Map<Product>(p)).ToListAsync(ct);
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