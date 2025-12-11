using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;

namespace Volunteers.Application.VolunteersFeatures.Update.SocialNetworks;

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