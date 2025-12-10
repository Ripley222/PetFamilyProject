using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;

public class FileDtoValidator : AbstractValidator<FileDto>
{
    public FileDtoValidator()
    {
        RuleFor(f => f.Stream)
            .NotNull()
            .WithError(Errors.GeneralErrors.ValueIsRequired("Stream"));
        
        RuleFor(f => f.FileName)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("FileName"));
    }
}