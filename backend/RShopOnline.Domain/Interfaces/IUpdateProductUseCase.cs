using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface IUpdateProductUseCase
{
    Task<Product> Handle(UpdateProductCommand command, CancellationToken ct);
}