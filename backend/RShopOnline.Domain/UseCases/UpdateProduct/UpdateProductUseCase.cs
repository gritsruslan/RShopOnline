using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.UpdateProduct;

public class UpdateProductUseCase(
    IGetProductsStorage getStorage, 
    IGetCategoriesStorage getCategoriesStorage, 
    IUpdateProductStorage updateStorage) : IUpdateProductUseCase
{
    public async Task<Product> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var doesProductExist = await getStorage.DoesProductExist(command.Id, ct);
        if (!doesProductExist)
        {
            throw new Exception("Product does not exist"); //TODO Result
        }
        
        var doesNewCategoryExist = await getCategoriesStorage.DoesCategoryExist(command.CategoryId, ct);
        if (!doesNewCategoryExist)
        {
            throw new Exception("Category does not exist"); //TODO Result
        }

        return await updateStorage.UpdateProduct(command.Id, command.Name, command.Price, command.InStock,
            command.Description, ct);
    }
}