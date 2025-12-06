using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Options;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.GetAll;

public class GetPetsHandler(
    IReadDbContext readDbContext,
    IFileProvider fileProvider,
    IMinioBucketOptions bucketOptions,
    IValidator<GetPetsQuery> validator)
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

        if (query.SpeciesName is not null)
        {
            var speciesId = await readDbContext.SpeciesRead
                .Where(s => s.Name == query.SpeciesName)
                .Select(s => s.Id)
                .FirstOrDefaultAsync(cancellationToken);

            petsQuery = petsQuery.Where(p => p.SpeciesBreed.SpeciesId == speciesId);
        }

        if (query.BreedsName is not null)
        {
            var breedId = await readDbContext.BreedsRead
                .Where(b => b.Name == query.BreedsName)
                .Select(b => b.Id)
                .FirstOrDefaultAsync(cancellationToken);

            petsQuery = petsQuery.Where(p => p.SpeciesBreed.BreedId == breedId);
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

        var speciesDictionary = await GetSpeciesDictionary(petsData, cancellationToken);
        var breedsDictionary = await GetBreedsDictionary(petsData, cancellationToken);

        var presignedUrlsDictionary = await GetPresignedUrlsDictionary(petsData, cancellationToken);

        var pets = petsData.Select(p => new PetDto(
                p.PetId,
                p.VolunteerId,
                speciesDictionary[p.SpeciesId],
                breedsDictionary[p.BreedId],
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

    private async Task<Dictionary<Guid, string>> GetSpeciesDictionary(
        IReadOnlyList<PetData> petsData, CancellationToken cancellationToken = default)
    {
        List<SpeciesId> speciesIdsList = [];
        speciesIdsList.AddRange(petsData.Select(data => SpeciesId.Create(data.SpeciesId)));

        var speciesDictionary = await readDbContext.SpeciesRead
            .Where(s => speciesIdsList.Contains(s.Id))
            .ToDictionaryAsync(
                s => s.Id.Value,
                s => s.Name,
                cancellationToken);

        return speciesDictionary;
    }

    private async Task<Dictionary<Guid, string>> GetBreedsDictionary(
        IReadOnlyList<PetData> petsData, CancellationToken cancellationToken = default)
    {
        List<BreedId> breedsIdsList = [];
        breedsIdsList.AddRange(petsData.Select(data => BreedId.Create(data.BreedId)));

        var breedsDictionary = await readDbContext.BreedsRead
            .Where(b => breedsIdsList.Contains(b.Id))
            .ToDictionaryAsync(
                b => b.Id.Value,
                b => b.Name,
                cancellationToken);

        return breedsDictionary;
    }
}