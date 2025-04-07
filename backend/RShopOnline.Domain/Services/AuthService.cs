using Microsoft.AspNetCore.Mvc;
using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Storage.Interfaces;
using EmptyResult = RShopAPI_Test.Core.Common.EmptyResult;

namespace RShopAPI_Test.Services.Services;

public class AuthService(
    IUsersRepository repository, 
    ISaltGenerator saltGenerator, 
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<EmptyResult> Registration(RegistrationCommand command, CancellationToken ct)
    {
        /*var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            return new Error(validationResult.Errors[0].ErrorMessage);
        }*/
        
        bool doesUserExist = await repository.UserExists(command.Email, ct);
        if (doesUserExist)
        {
            return new Error("User with such an email already exists");
        }
        
        byte[] salt = saltGenerator.Generate();
        byte[] passwordHash = passwordHasher.HashPassword(command.Password, salt);
        
        await repository.CreateUser(
            command.Email, 
            command.Password, 
            passwordHash, salt, 
            Role.Customer, 
            ct);

        return EmptyResult.Success();
    }

    public async Task<Result<string>> Login(LoginCommand command, CancellationToken ct)
    {
        const string errorMessage = "Invalid username or password";
        
        /*var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            return new Error(errorMessage);
        }*/
        
        var candidate = await repository.GetUserByEmail(command.Email, ct);
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

        return jwtProvider.GenerateToken(candidate);
    }
}