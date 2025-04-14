using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class ProductService(
    IProductsRepository productsRepository,
    ICategoriesRepository categoriesRepository) : IProductService
{
    public async Task<Result<Product>> CreateProduct(CreateProductCommand command, CancellationToken ct)
    {
        var categoryExist = await categoriesRepository.CategoryExists(command.CategoryId, ct);

        if (!categoryExist)
        {
            return new Error("Category doesn't exist");
        }
        
        return await productsRepository.CreateProduct(command.Name, command.Price, command.CategoryId, command.InStock,
            command.Description, ct);
    }

    public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct)
    {
        return await productsRepository.GetAllProducts(ct);
    }

    public async Task<Result<Product>> GetProduct(Guid id, CancellationToken ct)
    {
        var product = await productsRepository.GetProductById(id, ct);
        if (product is null)
        {
            return new Error("Product not found");
        }
        return product;
    }
    
    private readonly IReadOnlySet<string> _productFields = 
        new HashSet<string> {"Name", "Price", "InStock"};

    public async Task<Result<IEnumerable<Product>>> GetProducts(GetProductsCommand command, CancellationToken ct)
    {
        bool categoryExists = await categoriesRepository.CategoryExists(command.CategoryId, ct);

        if (!categoryExists)
        {
            return new Error("Category doesn't exist");
        }

        if (!_productFields.Contains(command.OrderByField))
        {
            return new Error("Incorrect sort field!");
        }

        if (command.Page <= 0)
        {
            return new Error("Incorrect page number!");
        }

        if (command.PageSize <= 0)
        {
            return new Error("Incorrect page size!");
        }
        
        int skip = (command.Page - 1) * command.PageSize;
        int take = command.PageSize;
        
        var products = await productsRepository
            .GetProducts(command.CategoryId, skip, take, command.OrderByField, command.Ascending, ct);
        
        // idk why implicit operator is not working here
        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<Product>> UpdateProduct(UpdateProductCommand command, CancellationToken ct)
    {
        var productExists = await productsRepository.ProductExists(command.Id, ct);
        if (!productExists)
        {
            return new Error("Product does not exist");
        }
        
        var categoryExists = await categoriesRepository.CategoryExists(command.CategoryId, ct);
        if (!categoryExists)
        {
            return new Error("Category does not exist");
        }

        return await productsRepository.UpdateProduct(command.Id, command.Name, command.Price, command.InStock,
            command.Description, ct);
    }
}