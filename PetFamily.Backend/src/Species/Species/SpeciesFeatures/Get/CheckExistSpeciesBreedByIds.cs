using Application.Abstraction;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Species.Contracts.DTOs;
using Species.Database;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.SpeciesFeatures.Get;

public sealed class CheckExistSpeciesBreedByIds
{
    public record CheckExistSpeciesBreedByIdsQuery(GetSpeciesBreedByIdsDto Dto) : IQuery;

    public sealed class CheckExistsSpeciesBreedByIdsHandler(
        SpeciesDbContext dbContext) : IQueryHandler<CheckExistSpeciesBreedByIdsQuery>
    {
        public async Task<UnitResult<ErrorList>> Handle(
            CheckExistSpeciesBreedByIdsQuery query, CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.Create(query.Dto.SpeciesId);
            var breedId = BreedId.Create(query.Dto.BreedId);

            var species = await dbContext.Species
                .Include(s => s.Breeds.Where(b => b.Id == breedId))
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();

            var breed = species.Breeds.FirstOrDefault(b => b.Id == breedId);

            if (breed is null)
                return Errors.BreedsErrors.NotFound().ToErrorList();

            return UnitResult.Success<ErrorList>();
        }
    }
}