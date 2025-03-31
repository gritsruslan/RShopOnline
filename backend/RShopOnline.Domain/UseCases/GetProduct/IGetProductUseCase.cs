using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.GetProduct;

public interface IGetProductUseCase
{
    Task<Result<Product>> Handle(Guid id, CancellationToken ct);
}