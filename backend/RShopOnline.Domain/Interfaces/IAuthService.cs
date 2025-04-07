using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Interfaces;

public interface IAuthService
{
    Task<EmptyResult> Registration(RegistrationCommand command, CancellationToken ct);
    
    Task<Result<string>> Login(LoginCommand command, CancellationToken ct);
}