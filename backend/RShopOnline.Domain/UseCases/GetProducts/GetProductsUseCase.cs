using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.GetProducts;

public class GetProductsUseCase(
    IGetProductsStorage productsStorage, 
    IGetCategoriesStorage categoriesStorage) : IGetProductsUseCase
{
    private readonly IReadOnlySet<string> _productFields = 
        new HashSet<string> {"Name", "Price", "InStock"};
        
    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsCommand command, CancellationToken ct)
    {
        bool doesCategoryExist = await categoriesStorage.DoesCategoryExist(command.CategoryId, ct);

        if (!doesCategoryExist)
        {
            return new Error("Category doesn't exist");
        }

        if (!_productFields.Contains(command.OrderByField))
        {
            return new Error("Incorrect sort field!");
        }

        if (command.Page <= 0)
        {
            return new Error("Incorrect page number!");
        }

        if (command.PageSize <= 0)
        {
            return new Error("Incorrect page size!");
        }
        
        int skip = (command.Page - 1) * command.PageSize;
        int take = command.PageSize;
        
        var products = await productsStorage
            .GetProducts(command.CategoryId, skip, take, command.OrderByField, command.Ascending, ct);
        
        // idk why implicit operator is not working here
        return Result<IEnumerable<Product>>.Success(products);
    }
}