using FluentValidation;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        const string invalidPassword = "Invalid password";
        
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage(invalidPassword)
            .MinimumLength(8).WithMessage(invalidPassword)
            .MaximumLength(30).WithMessage(invalidPassword)
            .Matches("[A-Z]").WithMessage(invalidPassword)
            .Matches("[a-z]").WithMessage(invalidPassword)
            .Matches("[0-9]").WithMessage(invalidPassword)
            .NotEqual(c => c.NewPassword).WithMessage("Old and new passwords must be different!");
        
        RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("New Password can't be empty!")
            .MinimumLength(8).WithMessage("New Password minimum length is 8")
            .MaximumLength(30).WithMessage("New Password maximum length is 30!")
            .Matches("[A-Z]").WithMessage("New Password must contain at least one uppercase letter!")
            .Matches("[a-z]").WithMessage("New Password must contain at least one lowercase letter!")
            .Matches("[0-9]").WithMessage("New Password must contain at least one number!");
    }
}