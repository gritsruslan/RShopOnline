using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Repositories;

public class ProductsRepository(RShopDbContext dbContext, IMapper mapper) : IProductsRepository
{
    public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct)
    {
        return await dbContext.Products.AsNoTracking().Select(p => mapper.Map<Product>(p)).ToListAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetProducts(
        Guid categoryId, 
        int skip, int take, 
        string orderByField, 
        bool ascending, 
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

        if (ascending)
            query = query.OrderBy(orderByExpression);
        else
            query = query.OrderByDescending(orderByExpression);

        return await query.Select(p => mapper.Map<Product>(p)).ToListAsync(ct);
    }
    
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

    public async Task<Product?> GetProductById(Guid id, CancellationToken ct)
    {
        var productEntity = await dbContext.Products
            .AsNoTracking().
            FirstOrDefaultAsync(p => p.Id == id, ct); 
        return productEntity is null ? null : mapper.Map<Product>(productEntity);
    }

    public async Task<bool> ProductExists(Guid id, CancellationToken ct)
    {
        return await GetProductById(id, ct) != null;
    }
}