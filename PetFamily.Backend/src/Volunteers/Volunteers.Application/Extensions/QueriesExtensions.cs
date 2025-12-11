using Microsoft.EntityFrameworkCore;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity;

namespace Volunteers.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PetData?> MappingPetsData(
        this IQueryable<Pet> petsQuery, CancellationToken cancellationToken = default)
    {
        var petData = await petsQuery.Select(
                p => new PetData(
                    p.Id.Value,
                    p.VolunteerId.Value,
                    p.SpeciesBreed.SpeciesId,
                    p.SpeciesBreed.BreedId,
                    p.Name.Value,
                    p.Description.Value,
                    p.Color,
                    p.HealthInformation.Value,
                    p.Address.City,
                    p.Address.Street,
                    p.Address.House,
                    p.BodySize.Weight,
                    p.BodySize.Height,
                    p.PhoneNumber.Value,
                    p.IsNeutered,
                    p.DateOfBirth,
                    p.IsVaccinated,
                    p.HelpStatus.Value,
                    p.Requisites.Select(r =>
                        new RequisitesDto
                        {
                            AccountNumber = r.AccountNumber,
                            Description = r.Description,
                            Title = r.Title
                        }),
                    p.MainFile.Value))
            .FirstOrDefaultAsync(cancellationToken);
        
        return petData;
    }
    
    public static async Task<IReadOnlyList<PetData>> MappingPetsDataToList(
        this IQueryable<Pet> petsQuery, CancellationToken cancellationToken = default)
    {
        var petData = await petsQuery.Select(
                p => new PetData(
                    p.Id.Value,
                    p.VolunteerId.Value,
                    p.SpeciesBreed.SpeciesId,
                    p.SpeciesBreed.BreedId,
                    p.Name.Value,
                    p.Description.Value,
                    p.Color,
                    p.HealthInformation.Value,
                    p.Address.City,
                    p.Address.Street,
                    p.Address.House,
                    p.BodySize.Weight,
                    p.BodySize.Height,
                    p.PhoneNumber.Value,
                    p.IsNeutered,
                    p.DateOfBirth,
                    p.IsVaccinated,
                    p.HelpStatus.Value,
                    p.Requisites.Select(r =>
                        new RequisitesDto
                        {
                            AccountNumber = r.AccountNumber,
                            Description = r.Description,
                            Title = r.Title
                        }),
                    p.MainFile.Value))
            .ToListAsync(cancellationToken);
        
        return petData;
    }

    public static IQueryable<Pet> SortingPets(this IQueryable<Pet> petsQuery)
    {
        petsQuery = petsQuery
            .OrderBy(p => p.Name.Value)
            .ThenBy(p => p.VolunteerId)
            .ThenBy(p => p.DateOfBirth)
            .ThenBy(p => p.Color)
            .ThenBy(p => p.Address.City)
            .ThenBy(p => p.Address.Street)
            .ThenBy(p => p.Address.House);

        return petsQuery;
    }
    
    public static IQueryable<Pet> PaginationPets(
        this IQueryable<Pet> petsQuery, int pageNumber, int pageSize)
    {
        petsQuery = petsQuery
            .Skip(pageNumber * pageSize - pageSize)
            .Take(pageSize);

        return petsQuery;
    }
}