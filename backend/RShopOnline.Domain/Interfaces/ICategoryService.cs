using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface ICategoryService
{
    Task<EmptyResult> CreateCategory(CreateCategoryCommand command, CancellationToken ct);
    
    Task<IEnumerable<Category>> GetCategories(CancellationToken ct);
}