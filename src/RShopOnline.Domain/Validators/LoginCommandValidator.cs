using FluentValidation;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        const string message = "Email or password is incorrect.";
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(message)
            .EmailAddress().WithMessage(message);
        
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage(message)
            .MinimumLength(8).WithMessage(message)
            .MaximumLength(30).WithMessage(message)
            .Matches("[A-Z]").WithMessage(message)
            .Matches("[a-z]").WithMessage(message)
            .Matches("[0-9]").WithMessage(message);
    }
}