using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace RShopAPI_Test.Services.Validators.Services;

public class ValidatorFactory(IServiceProvider serviceProvider) : IValidatorFactory
{
    public async Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, CancellationToken ct)
    {
        var matchingValidator = serviceProvider.GetRequiredService<IValidator<TCommand>>();
        return await matchingValidator.ValidateAsync(command, ct);
    }
}