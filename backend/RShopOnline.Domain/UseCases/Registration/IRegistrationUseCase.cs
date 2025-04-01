using RShopAPI_Test.Core.Common;

namespace RShopAPI_Test.Services.UseCases.Registration;

public interface IRegistrationUseCase
{
    Task<EmptyResult> Handle(RegistrationCommand command, CancellationToken ct);
}