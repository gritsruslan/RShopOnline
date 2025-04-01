using FluentValidation;
using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.LoginUseCase;

public class LoginUseCase(
    IGetUserStorage storage,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IValidator<LoginCommand> validator) : ILoginUseCase
{
    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken ct)
    {
        const string errorMessage = "Invalid username or password";
        
        /*var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            return new Error(errorMessage);
        }*/
        
        var candidate = await storage.GetUser(command.Email, ct);
        if (candidate is null)
        {
            return new Error(errorMessage);
        }

        bool isCorrectPassword =
            passwordHasher.VerifyPassword(command.Password, candidate.PasswordHash, candidate.Salt);

        if (!isCorrectPassword)
        {
            return new Error(errorMessage);
        }

        return jwtTokenService.GenerateToken(candidate);
    }
}