using Application.Abstraction;
using Application.Extensions;
using Application.Options;
using CSharpFunctionalExtensions;
using FluentValidation;
using SharedKernel;
using Species.Contracts;
using Species.Contracts.DTOs;
using Volunteers.Application.Database;
using Volunteers.Application.Extensions;
using Volunteers.Application.FileProvider;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using IFileProvider = Volunteers.Application.Providers.IFileProvider;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetById;

public class GetPetsByIdHandler(
    IReadDbContext readDbContext,
    IFileProvider fileProvider,
    ISpeciesContract speciesContract,
    IMinioBucketOptions options,
    IValidator<GetPetsByIdQuery> validator) : IQueryHandler<PetDto, GetPetsByIdQuery>
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
            return Errors.PetErrors.NotFound().ToErrorList();
        
        var speciesBreedResult = await speciesContract.GetSpeciesBreedsInfoByIds(
            new GetSpeciesBreedByIdsDto(petData.SpeciesId, petData.BreedId), cancellationToken);

        var filePath = FilePath.ParseOrGenerate(petData.MainFile);
        var fileData = new FileData(filePath, options.BucketPhotos);
        
        var fileLink = await fileProvider.GetFileLink(fileData, cancellationToken);
        if (fileLink.IsFailure)
            fileLink = string.Empty;

        var pet = new PetDto(
            petData.PetId,
            petData.VolunteerId,
            speciesBreedResult.IsSuccess ? speciesBreedResult.Value.SpeciesName : string.Empty,
            speciesBreedResult.IsSuccess ? speciesBreedResult.Value.BreedName : string.Empty,
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