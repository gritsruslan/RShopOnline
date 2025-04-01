using RShopAPI_Test.Core.Common;

namespace RShopAPI_Test.Services.UseCases.LoginUseCase;

public interface ILoginUseCase
{
    Task<Result<string>> Handle(LoginCommand command, CancellationToken ct);
}