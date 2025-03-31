namespace RShopAPI_Test.Services.Interfaces;

public interface IDeleteProductUseCase
{
    Task Delete(Guid id, CancellationToken ct);
}