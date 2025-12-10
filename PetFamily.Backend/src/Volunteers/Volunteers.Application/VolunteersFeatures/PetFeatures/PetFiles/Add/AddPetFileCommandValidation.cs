using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;

public class AddPetFileCommandValidation : AbstractValidator<AddPetFileCommand>
{
    public AddPetFileCommandValidation()
    {
        RuleFor(a => a.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Errors.GeneralErrors.ValueIsRequired("VolunteerId"));
        
        RuleFor(a => a.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Errors.GeneralErrors.ValueIsRequired("PetId"));
        
        RuleFor(a => a.Files)
            .NotEmpty();

        RuleForEach(a => a.Files)
            .SetValidator(new FileDtoValidator());
    }
}