using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));

        RuleForEach(u => u.SocialNetworks)
            .SetValidator(new CreateSocialNetworksDtoValidator());
    }
}