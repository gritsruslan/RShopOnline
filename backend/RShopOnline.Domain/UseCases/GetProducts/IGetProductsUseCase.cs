using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Interfaces;

public interface IGetProductsUseCase
{
    Task<IEnumerable<Product>> Handle(CancellationToken ct);
}