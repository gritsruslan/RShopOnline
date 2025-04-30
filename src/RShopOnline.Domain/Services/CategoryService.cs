using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.Services;

public class CategoryService(IIntentionManager intentionManager, ICategoriesRepository repository) : ICategoryService
{
    public async Task<EmptyResult> CreateCategory(CreateCategoryCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<CreateCategoryCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        await repository.CreateCategory(command.Name, ct);
        return EmptyResult.Success();
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken ct)
    {
        return await repository.GetCategories(ct);
    }
}