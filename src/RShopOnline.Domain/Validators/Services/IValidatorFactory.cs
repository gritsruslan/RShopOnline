using FluentValidation;
using FluentValidation.Results;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Validators.Services;

public interface IValidatorFactory
{
    Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, CancellationToken ct); 
}