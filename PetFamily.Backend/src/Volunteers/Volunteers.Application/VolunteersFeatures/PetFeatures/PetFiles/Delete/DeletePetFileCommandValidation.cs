using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public class DeletePetFileCommandValidation : AbstractValidator<DeletePetFileCommand>
{
    public DeletePetFileCommandValidation()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("VolunteerId"));
        
        RuleFor(d => d.PetId)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("PetId"));
        
        RuleFor(d => d.FileName)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("FileName"));
    }
}