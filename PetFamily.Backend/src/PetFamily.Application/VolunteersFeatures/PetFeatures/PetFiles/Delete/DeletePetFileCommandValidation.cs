using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public class DeletePetFileCommandValidation : AbstractValidator<DeletePetFileCommand>
{
    public DeletePetFileCommandValidation()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(d => d.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(d => d.FileName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("FileName"));
    }
}