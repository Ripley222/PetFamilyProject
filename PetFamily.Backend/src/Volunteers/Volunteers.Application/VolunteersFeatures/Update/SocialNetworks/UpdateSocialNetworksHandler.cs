using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Update.SocialNetworks;

public class UpdateSocialNetworksHandler(
    IVolunteersRepository repository,
    IValidator<UpdateSocialNetworksCommand> validator,
    ILogger<UpdateSocialNetworksHandler> logger) : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();

        var socialNetworks =
            command.SocialNetworks.Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value);

        resultVolunteer.Value.UpdateSocialNetworks(socialNetworks);

        var saveResult = await repository.Save(resultVolunteer.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();

        logger.LogInformation("Updates social networks volunteer with id {volunteerId}", command.VolunteerId);

        return saveResult.Value;
    }
}