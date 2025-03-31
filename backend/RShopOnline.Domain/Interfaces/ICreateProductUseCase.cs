using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface ICreateProductUseCase
{
    Task<Product> Handle(CreateProductCommand command, CancellationToken ct);
}