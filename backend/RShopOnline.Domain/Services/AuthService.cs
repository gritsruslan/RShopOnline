using FluentValidation;
using RShopAPI_Test.Core.Common;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authorization;
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
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager,
    
    //TODO : Fix validators dep injection
    IValidator<RegistrationCommand> registrationValidator,
    IValidator<LoginCommand> loginValidator,
    IValidator<ChangePasswordCommand> changePasswordValidator,
    
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<EmptyResult> Registration(RegistrationCommand command, CancellationToken ct)
    {
        /*var validationResult = await registrationValidator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            return new Error(validationResult.Errors[0].ErrorMessage);
        }*/
        
        bool userExists = await repository.UserExists(command.Email, ct);
        if (userExists)
        {
            return new Error("User with such an email already exists");
        }
        
        byte[] salt = saltGenerator.Generate();
        byte[] passwordHash = passwordHasher.HashPassword(command.Password, salt);
        
        await repository.CreateUser(
            command.Name, 
            command.Email, 
            passwordHash, 
            salt, 
            Role.Customer, 
            ct);

        return EmptyResult.Success();
    }

    public async Task<Result<string>> Login(LoginCommand command, CancellationToken ct)
    {
        const string errorMessage = "Invalid username or password";
        
        /*var validationResult = await loginValidator.ValidateAsync(command, ct);
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

    public async Task<EmptyResult> ChangePassword(ChangePasswordCommand command, CancellationToken ct)
    {
        if (!intentionManager.IsAllowed<ChangePasswordCommand>())
        {
            return new Error(ErrorCode.Forbidden);
        }
        
        const string invalidPasswordMessage = "Invalid password!";
        var validationResult = await changePasswordValidator.ValidateAsync(command, ct);
        /*if (!validationResult.IsValid)
        {
            return new Error(invalidPasswordMessage);
        }*/

        var userId = identityProvider.Current.Id;
        var user = await repository.GetUserById(userId, ct);
        if (user is null)
        {
            return new Error(ErrorCode.Unauthorized);
        }
        
        bool isCorrectPassword = passwordHasher.VerifyPassword(command.Password, user.PasswordHash, user.Salt);

        if (!isCorrectPassword)
        {
            return new Error("Invalid password");
        }
        
        var newPasswordHash = passwordHasher.HashPassword(command.NewPassword, user.Salt);
        await repository.UpdatePassword(newPasswordHash, ct);
        
        return EmptyResult.Success();
    }
}