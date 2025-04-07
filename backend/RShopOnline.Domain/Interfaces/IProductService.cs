using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface IProductService
{
    Task<Result<Product>> CreateProduct(CreateProductCommand command, CancellationToken ct);
    
    Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct);
    
    Task<Result<Product>> GetProduct(Guid id, CancellationToken ct);
    
    Task<Result<IEnumerable<Product>>> GetProducts(GetProductsCommand command, CancellationToken ct);

    public Task<Result<Product>> UpdateProduct(UpdateProductCommand command, CancellationToken ct);
}