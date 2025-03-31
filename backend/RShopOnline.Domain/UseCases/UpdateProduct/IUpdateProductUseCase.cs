using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.UpdateProduct;

public interface IUpdateProductUseCase
{
    Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken ct);
}