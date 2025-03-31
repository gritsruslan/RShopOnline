using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.UpdateProduct;

public class UpdateProductUseCase(
    IGetProductsStorage getStorage, 
    IGetCategoriesStorage getCategoriesStorage, 
    IUpdateProductStorage updateStorage) : IUpdateProductUseCase
{
    public async Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var doesProductExist = await getStorage.DoesProductExist(command.Id, ct);
        if (!doesProductExist)
        {
            return new Error("Product does not exist");
        }
        
        var doesNewCategoryExist = await getCategoriesStorage.DoesCategoryExist(command.CategoryId, ct);
        if (!doesNewCategoryExist)
        {
            return new Error("Category does not exist");
        }

        return await updateStorage.UpdateProduct(command.Id, command.Name, command.Price, command.InStock,
            command.Description, ct);
    }
}