using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.Delete;

public class DeleteSpeciesHandler(
    IReadDbContext readDbContext,
    IValidator<DeleteSpeciesCommand> validator,
    ILogger<DeleteSpeciesHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        
        var petQuery = readDbContext.PetsRead;
        
        petQuery = petQuery.Where(p => p.SpeciesBreed.SpeciesId == speciesId);
        if (petQuery.Any())
            return Error.Failure("species.used", "Species is used").ToErrorList();
        
        var speciesQuery = readDbContext.SpeciesRead;
        
        speciesQuery = speciesQuery.Where(s => s.Id == speciesId);

        await speciesQuery.ExecuteDeleteAsync(cancellationToken);
        
        logger.LogInformation("Species with id {speciesId} has been deleted", speciesId.Value);
        
        return speciesId.Value;
    }
}