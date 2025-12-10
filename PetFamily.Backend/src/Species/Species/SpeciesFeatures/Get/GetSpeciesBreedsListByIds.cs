using Application.Abstraction;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Species.Contracts.DTOs;
using Species.Database;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.SpeciesFeatures.Get;

public sealed class GetSpeciesBreedsListByIds
{
    public record GetSpeciesBreedsListByIdsQuery(IEnumerable<GetSpeciesBreedByIdsDto> dtos) : IQuery;
    
    public sealed class GetSpeciesBreedsListByIdsHandler(SpeciesDbContext dbContext) :
        IQueryHandler<IReadOnlyList<SpeciesBreedDto>, GetSpeciesBreedsListByIdsQuery>
    {
        public async Task<Result<IReadOnlyList<SpeciesBreedDto>, ErrorList>> Handle(
            GetSpeciesBreedsListByIdsQuery query, CancellationToken cancellationToken = default)
        {
            var listSpeciesIds = query.dtos.Select(dto => SpeciesId.Create(dto.SpeciesId));
            var listBreedsIds = query.dtos.Select(dto => BreedId.Create(dto.BreedId));

            var species = await dbContext.Species
                .Where(s => listSpeciesIds.Contains(s.Id))
                .Include(s => s.Breeds.Where(b => listBreedsIds.Contains(b.Id)))
                .ToListAsync(cancellationToken);

            var dtoList = species.SelectMany(s => s.Breeds
                    .Select(b => new SpeciesBreedDto(s.Id.Value, b.Id.Value, s.Name, b.Name)))
                .ToList();

            return dtoList;
        }
    }
}

