using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.SocialNetworks;

public class UpdateSocialNetworksHandler(
    IVolunteersRepository repository,
    IValidator<UpdateSocialNetworksCommand> validator,
    ILogger<UpdateSocialNetworksHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.GetErrors();
        }

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();

        var socialNetworks = 
            command.SocialNetworks.Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value);

        resultVolunteer.Value.UpdateSocialNetworks(socialNetworks);

        var result = await repository.Save(resultVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Updates social networks volunteer with id {volunteerId}", resultVolunteer.Value.Id);

        return result;
    }
}