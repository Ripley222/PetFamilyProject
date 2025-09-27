using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.BreedsFeatures.Delete;

public class DeleteBreedsHandler(
    IReadDbContext readDbContext,
    ISpeciesRepository speciesRepository,
    IValidator<DeleteBreedsCommand> validator,
    ILogger<DeleteBreedsHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedsCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var breedId = BreedId.Create(command.BreedId);

        var petQuery = readDbContext.PetsRead;

        petQuery = petQuery.Where(p => p.SpeciesBreed.BreedId == breedId);
        if (petQuery.Any())
            return Error.Failure("breed.used", "This breed is used").ToErrorList();

        var breedQuery = readDbContext.BreedsRead;

        var breed = await breedQuery
            .FirstOrDefaultAsync(b => b.Id == breedId, cancellationToken);

        if (breed is null)
            return Error.NotFound("breed.not.found", "Breed not found").ToErrorList();

        var deletedResult = await speciesRepository.RemoveBreed(breed, cancellationToken);
        if (deletedResult.IsFailure)
            return deletedResult.Error.ToErrorList();

        logger.LogInformation("Breed with id {breedId} has been deleted", breedId.Value);

        return breedId.Value;
    }
}