using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;

public class FileDtoValidator : AbstractValidator<FileDto>
{
    public FileDtoValidator()
    {
        RuleFor(f => f.Stream)
            .NotNull()
            .WithError(Errors.General.ValueIsRequired("Stream"));
        
        RuleFor(f => f.FileName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("FileName"));
    }
}