using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Options;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;
using IFileProvider = PetFamily.Application.Providers.IFileProvider;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.GetById;

public class GetPetsByIdHandler(
    IReadDbContext readDbContext,
    IFileProvider fileProvider,
    IMinioBucketOptions options,
    IValidator<GetPetsByIdQuery> validator)
{
    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetsByIdQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var petsQuery = readDbContext.PetsRead;
        petsQuery = petsQuery.Where(p => p.Id == PetId.Create(query.PetId));

        var petData = await petsQuery.MappingPetsData(cancellationToken);
        if (petData is null)
            return Errors.Pet.NotFound().ToErrorList();

        var species = await readDbContext.SpeciesRead
            .FirstOrDefaultAsync(s => s.Id == SpeciesId.Create(petData.SpeciesId), cancellationToken);
        
        if (species is null)
            return Errors.Species.NotFound().ToErrorList();
        
        var breed = await readDbContext.BreedsRead
            .FirstOrDefaultAsync(b => b.Id == BreedId.Create(petData.BreedId), cancellationToken);
        
        if (breed is null)
            return Errors.Breeds.NotFound().ToErrorList();

        var filePath = FilePath.ParseOrGenerate(petData.MainFile);
        var fileData = new FileData(filePath, options.BucketPhotos);
        
        var fileLink = await fileProvider.GetFileLink(fileData, cancellationToken);
        if (fileLink.IsFailure)
            fileLink = string.Empty;

        var pet = new PetDto(
            petData.PetId,
            petData.VolunteerId,
            species.Name,
            breed.Name,
            petData.Name,
            petData.Description,
            petData.Color,
            petData.HealthInformation,
            petData.City,
            petData.Street,
            petData.House,
            petData.Weight,
            petData.Height,
            petData.PhoneNumber,
            petData.IsNeutered,
            petData.DateOfBirth,
            petData.IsVaccinated,
            petData.HelpStatus,
            petData.Requisites,
            fileLink.Value);
        
        return pet;
    }
}