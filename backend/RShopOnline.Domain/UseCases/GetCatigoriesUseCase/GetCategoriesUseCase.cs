using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;

public class GetCategoriesUseCase(IGetCategoriesStorage storage) : IGetCategoriesUseCase
{
    public async Task<IEnumerable<Category>> Handle(CancellationToken ct)
    {
        return await storage.GetCategories(ct);
    }
}