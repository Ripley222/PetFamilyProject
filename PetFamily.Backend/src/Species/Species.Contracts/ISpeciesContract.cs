using CSharpFunctionalExtensions;
using SharedKernel;
using Species.Contracts.DTOs;

namespace Species.Contracts;

public interface ISpeciesContract
{
    Task<Result<SpeciesBreedDto, ErrorList>> GetSpeciesBreedsInfoByIds(
        GetSpeciesBreedByIdsDto dto, CancellationToken cancellationToken = default);
    
    Task<Result<IReadOnlyList<SpeciesBreedDto>, ErrorList>> GetSpeciesBreedsInfoListByIds(
        IEnumerable<GetSpeciesBreedByIdsDto> dtos, CancellationToken cancellationToken = default);
    
    Task<UnitResult<ErrorList>> CheckExistSpeciesBreedsByIds(
        GetSpeciesBreedByIdsDto dto, CancellationToken cancellationToken = default);
}