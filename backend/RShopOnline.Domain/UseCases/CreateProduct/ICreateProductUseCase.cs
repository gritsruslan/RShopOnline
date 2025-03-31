using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.UseCases.CreateProduct;

public interface ICreateProductUseCase
{
    Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken ct);
}