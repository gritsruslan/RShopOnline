using FluentValidation;
using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Services.UseCases.Registration;

public class RegistrationUseCase(
    ICreateUserStorage createUserStorage, 
    IGetUserStorage getUserStorage,
    IPasswordHasher passwordHasher,
    ISaltGenerator saltGenerator,
    IValidator<RegistrationCommand> validator) : IRegistrationUseCase
{
    public async Task<EmptyResult> Handle(RegistrationCommand command, CancellationToken ct)
    {
        /*var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            return new Error(validationResult.Errors[0].ErrorMessage);
        }*/
        
        bool doesUserExist = await getUserStorage.DoesUserExist(command.Email, ct);
        if (doesUserExist)
        {
            return new Error("User with such an email already exists");
        }
        
        byte[] salt = saltGenerator.Generate();
        byte[] passwordHash = passwordHasher.HashPassword(command.Password, salt);
        
        await createUserStorage.CreateUser(
            command.Email, 
            command.Password, 
            passwordHash, salt, 
            UserRole.Customer, 
            ct);

        return EmptyResult.Success();
    }
}