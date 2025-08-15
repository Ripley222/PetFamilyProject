using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;

public class GetPetFileLinkCommandValidation : AbstractValidator<GetPetFileLinkCommand>
{
    public GetPetFileLinkCommandValidation()
    {
        RuleFor(a => a.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(a => a.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(a => a.FileName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("FileName"));
    }
}