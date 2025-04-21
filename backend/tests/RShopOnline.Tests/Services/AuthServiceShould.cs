using System.Security.Cryptography;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopOnline.Tests.Services;

public class AuthServiceShould
{
    private readonly AuthService Sut;
    private readonly ISetup<IUsersRepository, Task<bool>> UserExistsByEmailSetup;
    private readonly ISetup<IUsersRepository, Task<User?>> GetUserByIdSetup;
    private readonly ISetup<IUsersRepository, Task<User?>> GetUserByEmailSetup;
    
    private readonly IPasswordHasher PasswordHasher = new PasswordHasher();
    private readonly ISaltGenerator SaltGenerator = new SaltGenerator();

    public AuthServiceShould()
    {
        var usersRepository = new Mock<IUsersRepository>();
        var jwtProvider = new Mock<IJwtProvider>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(r => r.Current).Returns(
            new UserIdentity(Guid.Parse("960D90CB-014A-474D-A250-4520A795D091"), Role.Customer));
        
        var intentionManager = new Mock<IIntentionManager>();
        intentionManager.Setup(m => m.IsAllowed<ChangePasswordCommand>()).Returns(true);
            
        var registrationValidator = new Mock<IValidator<RegistrationCommand>>();
        var loginValidator = new Mock<IValidator<LoginCommand>>();
        var changePasswordValidator = new Mock<IValidator<ChangePasswordCommand>>();

        registrationValidator
            .Setup(r => r.ValidateAsync(It.IsAny<RegistrationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        loginValidator
            .Setup(r => r.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        changePasswordValidator
            .Setup(r => r.ValidateAsync(It.IsAny<ChangePasswordCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        Sut = new AuthService(
            usersRepository.Object, 
            SaltGenerator,
            PasswordHasher, 
            identityProvider.Object,
            intentionManager.Object,
            registrationValidator.Object, 
            loginValidator.Object, 
            changePasswordValidator.Object, 
            jwtProvider.Object);

        UserExistsByEmailSetup = usersRepository
            .Setup(r => r.UserExists(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        GetUserByIdSetup =
            usersRepository.Setup(r => r.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        GetUserByEmailSetup = 
            usersRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()));
    }
    

    [Fact]
    public async Task DontRegister_WhenUserWithSuchEmailAlreadyExists()
    {
        UserExistsByEmailSetup.ReturnsAsync(true);

        var result = await Sut.Registration(new RegistrationCommand("name", "email", "password"), 
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }


    [Fact]
    public async Task RegisterUser()
    {
        UserExistsByEmailSetup.ReturnsAsync(false);
        
        var result = await Sut.Registration(new RegistrationCommand("name", "email", "password"), 
            CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task DontLogin_WhenUserNotFound()
    {
        GetUserByEmailSetup.ReturnsAsync((User?)null);
        
        var result = await Sut.Login(new LoginCommand("email", "password"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public async Task DontLogin_WhenPasswordIsInvalid()
    {
        var salt = SaltGenerator.Generate();
        var password = "MyPassword";
        var passwordHash = new PasswordHasher().HashPassword(password, salt);
        GetUserByEmailSetup.ReturnsAsync(new User()
        {
            Name = "name",
            Email = "email",
            Salt = salt,
            PasswordHash = passwordHash
        });
        var incorrectPassword = password + "123";
        
        var result = await Sut.Login(new LoginCommand("email", incorrectPassword), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyLogin()
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var password = "MyPassword";
        var passwordHash = PasswordHasher.HashPassword(password, salt);
        GetUserByEmailSetup.ReturnsAsync(new User()
        {
            Name = "name",
            Email = "email",
            Salt = salt,
            PasswordHash = passwordHash
        });
        var result = await Sut.Login(new LoginCommand("email", password), CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task DontChangePassword_WhenUserDoesNotExists()
    {
        GetUserByIdSetup.ReturnsAsync((User?)null);

        var result = await Sut.ChangePassword(
            new ChangePasswordCommand("password", "newPassword"), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }


    [Fact]
    public async Task DontChangePassword_WhenPasswordIsInvalid()
    {
        var password = "MyPassword";
        var newPassword = "newPassword";
        var invalidPassword = "invalidPassword";
        
        var salt = SaltGenerator.Generate();
        var passwordHash = PasswordHasher.HashPassword(password, salt);
        
        GetUserByIdSetup.ReturnsAsync(new User
        {
            Name = "name",
            Email = "email",
            PasswordHash = passwordHash,
            Salt = salt
        });
        
        var result = await Sut.ChangePassword(
            new ChangePasswordCommand(invalidPassword, newPassword), CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task SuccessfullyChangePassword()
    {
        var password = "password";
        var newPassword = "newPassword";
        
        var salt = SaltGenerator.Generate();
        var passwordHash = PasswordHasher.HashPassword(password, salt);
        
        GetUserByIdSetup.ReturnsAsync(new User
        {
            Name = "name",
            Email = "email",
            PasswordHash = passwordHash,
            Salt = salt
        });

        var result = await Sut.ChangePassword(new ChangePasswordCommand(password, newPassword), CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }
}