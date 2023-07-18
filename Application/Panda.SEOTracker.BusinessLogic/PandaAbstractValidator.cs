using FluentValidation;

using Panda.SEOTracker.Dtos;
using Panda.SEOTracker.Dtos.Interfaces;

namespace Panda.SEOTracker.BusinessLogic;

public abstract class PandaAbstractValidator<T> : AbstractValidator<T>
{
    public bool IsValid(T item, out IValidationError[] errors)
    {
        var validationResult = Validate(item);

        if (validationResult.IsValid)
        {
            errors = Array.Empty<IValidationError>();
            return true;
        }

        errors = validationResult.Errors.Select(x => new ValidationError
        {
            AttemptedValue = x.AttemptedValue,
            Message = x.ErrorMessage,
            PropertyName = x.PropertyName
        }).ToArray();

        return false;
    }

    public async Task<IValidationError[]> IsValidAsync(T item)
    {
		var validationResult = await ValidateAsync(item);

        if (validationResult.IsValid)
        {
			return Array.Empty<IValidationError>();
        }

        return validationResult.Errors.Select(x => new ValidationError
        {
            AttemptedValue = x.AttemptedValue,
            Message = x.ErrorMessage,
            PropertyName = x.PropertyName
        }).ToArray();

    }
}