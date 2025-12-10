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
using Volunteers.Application.Providers;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetAll;

public class GetPetsHandler(
    IReadDbContext readDbContext,
    IFileProvider fileProvider,
    ISpeciesContract speciesContract,
    IMinioBucketOptions bucketOptions,
    IValidator<GetPetsQuery> validator) : IQueryHandler<IReadOnlyList<PetDto>, GetPetsQuery>
{
    public async Task<Result<IReadOnlyList<PetDto>, ErrorList>> Handle(
        GetPetsQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var petsQuery = readDbContext.PetsRead;

        if (query.VolunteerId is not null)
        {
            var volunteerId = VolunteerId.Create((Guid)query.VolunteerId);

            petsQuery = petsQuery.Where(p => p.VolunteerId == volunteerId);
        }

        if (query.PetName is not null)
        {
            petsQuery = petsQuery.Where(p => p.Name.Value.Contains(query.PetName));
        }

        if (query.PetAge is not null)
        {
            var today = DateTime.UtcNow;
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            petsQuery = petsQuery.Where(p =>
                p.DateOfBirth.ToDateTime(TimeOnly.MinValue) <= endOfYear.AddYears(-(int)query.PetAge) &&
                p.DateOfBirth.ToDateTime(TimeOnly.MinValue) >= startOfYear.AddYears(-(int)query.PetAge));
        }

        if (query.SpeciesId is not null)
        {
            petsQuery = petsQuery.Where(p => p.SpeciesBreed.SpeciesId == query.SpeciesId);
        }

        if (query.BreedId is not null)
        {
            petsQuery = petsQuery.Where(p => p.SpeciesBreed.BreedId == query.BreedId);
        }

        if (query.PetColor is not null)
        {
            petsQuery = petsQuery.Where(p => p.Color.Contains(query.PetColor));
        }

        if (query.PetCity is not null)
        {
            petsQuery = petsQuery.Where(p => p.Address.City.Contains(query.PetCity));
        }

        if (query.PetStreet is not null)
        {
            petsQuery = petsQuery.Where(p => p.Address.City.Contains(query.PetStreet));
        }

        if (query.PetHouse is not null)
        {
            petsQuery = petsQuery.Where(p => p.Address.House.Contains(query.PetHouse));
        }

        if (query.PetHealthInformation is not null)
        {
            petsQuery = petsQuery.Where(p => p.HealthInformation.Value.Contains(query.PetHealthInformation));
        }

        if (query.PetHelpStatus is not null)
        {
            petsQuery = petsQuery.Where(p => p.HelpStatus.Value.Contains(query.PetHelpStatus));
        }

        if (query.IsNeutered is not null)
        {
            petsQuery = petsQuery.Where(p => p.IsNeutered == query.IsNeutered);
        }

        if (query.IsVaccinated is not null)
        {
            petsQuery = petsQuery.Where(p => p.IsVaccinated == query.IsVaccinated);
        }

        petsQuery = petsQuery.SortingPets();
        petsQuery = petsQuery.PaginationPets(query.PageNumber, query.PageSize);

        var petsData = await petsQuery.MappingPetsDataToList(cancellationToken);

        var dtos = petsData
            .Select(data => new GetSpeciesBreedByIdsDto(data.SpeciesId, data.BreedId));

        var speciesBreedsResult = await speciesContract.GetSpeciesBreedsInfoListByIds(dtos, cancellationToken);

        var speciesDictionary = GetSpeciesDictionary(speciesBreedsResult.Value);
        var breedsDictionary = GetBreedsDictionary(speciesBreedsResult.Value);

        var presignedUrlsDictionary = await GetPresignedUrlsDictionary(petsData, cancellationToken);

        var pets = petsData.Select(p => new PetDto(
                p.PetId,
                p.VolunteerId,
                speciesDictionary.TryGetValue(p.SpeciesId, out var speciesName) ? speciesName : string.Empty,
                breedsDictionary.TryGetValue(p.BreedId, out var breedName) ? breedName : string.Empty,
                p.Name,
                p.Description,
                p.Color,
                p.HealthInformation,
                p.City,
                p.Street,
                p.House,
                p.Weight,
                p.Height,
                p.PhoneNumber,
                p.IsNeutered,
                p.DateOfBirth,
                p.IsVaccinated,
                p.HelpStatus,
                p.Requisites,
                presignedUrlsDictionary[p.MainFile ?? string.Empty]))
            .ToList();

        return Result.Success<IReadOnlyList<PetDto>, ErrorList>(pets);
    }

    private async Task<Dictionary<string, string>> GetPresignedUrlsDictionary(
        IReadOnlyList<PetData> petsData, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> presignedUrls = [];
        foreach (var petData in petsData)
        {
            var filePath = FilePath.ParseOrGenerate(petData.MainFile);
            var fileData = new FileData(filePath, bucketOptions.BucketPhotos);

            var presignedLinkResult = await fileProvider.GetFileLink(fileData, cancellationToken);
            if (presignedLinkResult.IsSuccess)
            {
                presignedUrls.TryAdd(petData.MainFile ?? string.Empty, presignedLinkResult.Value);
            }
            else
            {
                presignedUrls.TryAdd(string.Empty, string.Empty);
            }
        }

        return presignedUrls;
    }

    private static Dictionary<Guid, string> GetSpeciesDictionary(
        IReadOnlyList<SpeciesBreedDto> dtos)
    {
        var speciesDictionary = dtos.ToDictionary(
            k => k.SpeciesId,
            v => v.SpeciesName);

        return speciesDictionary;
    }

    private static Dictionary<Guid, string> GetBreedsDictionary(
        IReadOnlyList<SpeciesBreedDto> dtos)
    {
        var breedsDictionary = dtos.ToDictionary(
            k => k.BreedId,
            v => v.BreedName);

        return breedsDictionary;
    }
}