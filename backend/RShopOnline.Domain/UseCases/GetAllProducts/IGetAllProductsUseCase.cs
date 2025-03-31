using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.GetProducts;

public interface IGetAllProductsUseCase
{
    Task<IEnumerable<Product>> Handle(CancellationToken ct);
}