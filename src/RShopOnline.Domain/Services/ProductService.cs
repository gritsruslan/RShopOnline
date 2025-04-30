using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class ProductService(
    IProductsRepository productsRepository,
    ICategoriesRepository categoriesRepository,
    IImagesRepository imagesRepository,
    IImagesMinioStorage imagesStorage,
    IIntentionManager intentionManager) : IProductService
{
    private readonly IReadOnlySet<string> _productFields = new HashSet<string> {"Name", "Price", "InStock"};
    
    public async Task<Result<Product>> CreateProduct(CreateProductCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<CreateProductCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
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
            return new Error("Product not found", ErrorCode.NotFound);
        }
        return product;
    }
    
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
        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<Product>> UpdateProduct(UpdateProductCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<UpdateProductCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
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

    public async Task<Result<IEnumerable<string>>> GetProductImagesNames(Guid productId, CancellationToken ct)
    {
        var productExists = await productsRepository.ProductExists(productId, ct);
        if (!productExists)
        {
            return new Error("Product does not exist!");
        }
        
        var imageInfos = await imagesRepository.GetImagesByProductId(productId, ct);
        var imageNames = imageInfos.Select(i => i.Id + i.Extension);
        return Result<IEnumerable<string>>.Success(imageNames);
    }

    public async Task<Result<string>> AddProductImage(AddProductImageCommand command, CancellationToken ct)
    {
        var (formFile, productId) = command;
        var productExists = await productsRepository.ProductExists(productId, ct);
        if (!productExists)
        {
            return new Error("Product does not exist!");
        }
        
        var extension = Path.GetExtension(formFile.FileName);
        
        //add image to repository
        var imageId = await imagesRepository.AddImage(productId, extension, ct);
        var imageName = imageId + extension;
        
        // add image to minio storage
        await using var imageStream = formFile.OpenReadStream();
        await imagesStorage.UploadImage(imageStream, imageName, formFile.ContentType, formFile.Length, ct);

        return imageName;
    }

    public async Task<EmptyResult> DeleteProductImage(DeleteProductImageCommand command, CancellationToken ct)
    {
        var (productId, imageName) = command;

        var imageIdString = imageName.Substring(0, 35);
        if (!Guid.TryParse(imageIdString, out Guid imageId))
        {
            return new Error("Image doesn't exist!");
        }
        
        var productExists = await productsRepository.ProductExists(productId, ct);
        if (!productExists)
        {
            return new Error("Product does not exist!");
        }
        
        var imageExists = await imagesRepository.ImageExists(imageId, ct);
        if (!imageExists)
        {
            return new Error("Image doesn't exist!");
        }
        
        await imagesStorage.DeleteImage(imageName, ct);
        await imagesRepository.DeleteImage(imageId, ct);
        
        return EmptyResult.Success();
    }
}