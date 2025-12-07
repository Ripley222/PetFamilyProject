using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;

public class AddPetFileCommandValidation : AbstractValidator<AddPetFileCommand>
{
    public AddPetFileCommandValidation()
    {
        RuleFor(a => a.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(a => a.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(a => a.Files)
            .NotEmpty();

        RuleForEach(a => a.Files)
            .SetValidator(new FileDtoValidator());
    }
}