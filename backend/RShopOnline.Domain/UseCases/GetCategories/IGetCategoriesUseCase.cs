using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;

public interface IGetCategoriesUseCase
{
    Task<IEnumerable<Category>> Handle(CancellationToken ct);
}