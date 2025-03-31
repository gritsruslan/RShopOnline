using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.UseCases.CreateCategory;

public interface ICreateCategoryUseCase
{
    Task<Category> Handle(string name, CancellationToken ct);
}