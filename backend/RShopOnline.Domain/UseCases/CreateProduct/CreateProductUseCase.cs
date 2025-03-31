using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.CreateProduct;

public class CreateProductUseCase(ICreateProductStorage productStorage, IGetCategoriesStorage categoriesStorage) : ICreateProductUseCase
{
    public async Task<Product> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var doesCategoryExist = await categoriesStorage.DoesCategoryExist(command.CategoryId, ct);

        if (!doesCategoryExist) //TODO Result
        {
            throw new Exception("Category does not exist");
        }
        
        return await productStorage.CreateProduct(command.Name, command.Price, command.CategoryId, command.InStock,
            command.Description, ct);
    }
}