using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.CreateCategory;

public class CreateCategoryUseCase(ICreateCategoryStorage storage) : ICreateCategoryUseCase
{
    public async Task<Category> Handle(string name, CancellationToken ct)
    {
        return await storage.Create(name, ct);
    }
}