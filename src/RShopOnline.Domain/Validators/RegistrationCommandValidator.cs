using FluentValidation;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Validators;

public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
{
    public RegistrationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Incorrect email address!");
        
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password can't be empty!")
            .MinimumLength(8).WithMessage("Password minimum length is 8")
            .MaximumLength(30).WithMessage("Password maximum length is 30!")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter!")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter!")
            .Matches("[0-9]").WithMessage("Password must contain at least one number!");
        
        RuleFor(c => c.Name)
	        .NotEmpty().WithMessage("Name can't be empty!")
	        .MaximumLength(30).WithMessage("Maximum name length is 30!");
    }
}