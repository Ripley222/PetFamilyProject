using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationErrorsExtensions
{
    public static ErrorList GetErrors(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;
            
        var errors = validationErrors.Select(validationError
            => Error.Validation(
                validationError.ErrorCode, 
                validationError.ErrorMessage, 
                validationError.PropertyName));

        return new ErrorList(errors);
    }
}