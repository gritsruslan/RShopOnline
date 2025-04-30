using FluentAssertions;
using RShopAPI_Test.Services.Commands;
using RShopAPI_Test.Services.Validators;

namespace RShopOnline.Domain.Tests.Validators;

public class ChangePasswordCommandValidatorShould
{
    private readonly ChangePasswordCommandValidator sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var validCommand = new ChangePasswordCommand("Password1232", "Password123");
        var result = sut.Validate(validCommand);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnFailure_WhenCommandIsInvalid(ChangePasswordCommand command)
    {
        var result = sut.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new ChangePasswordCommand("Password1231", "Password123");
        //Password
        yield return [validCommand with { Password = string.Empty }];
        yield return [validCommand with { Password = "123" }];
        yield return [validCommand with { Password = string.Join(string.Empty, Enumerable.Range(0, 100).Select(n => "a")) }];
        yield return [validCommand with { Password = "password123" }];
        yield return [validCommand with { Password = "PASSWORD123" }];
        yield return [validCommand with { Password = "MyPassword" }];
        //NewPassword
        yield return [validCommand with { Password = string.Empty }];
        yield return [validCommand with { Password = "123" }];
        yield return [validCommand with { Password = string.Join(string.Empty, Enumerable.Range(0, 100).Select(n => "a")) }];
        yield return [validCommand with { Password = "password123" }];
        yield return [validCommand with { Password = "PASSWORD123" }];
        yield return [validCommand with { Password = "MyPassword" }];
        // Shared
        yield return [new ChangePasswordCommand(Password: "Password123", NewPassword: "Password123")];
    }
}