using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.GetProducts;

public interface IGetProductsUseCase
{
    Task<Result<IEnumerable<Product>>> Handle(GetProductsCommand command, CancellationToken ct);
}