using CSharpFunctionalExtensions;
using SharedKernel;
using Species.Contracts;
using Species.Contracts.DTOs;
using Species.SpeciesFeatures.Get;

namespace Species.Presenters;

public class SpeciesContract(
    CheckExistSpeciesBreedByIds.CheckExistsSpeciesBreedByIdsHandler checkExistHandler,
    GetSpeciesBreedsByIds.GetSpeciesBreedsByIdsHandler getSpeciesBreedsInfoHandler,
    GetSpeciesBreedsListByIds.GetSpeciesBreedsListByIdsHandler getListSpeciesBreedsInfoHandler) 
    : ISpeciesContract
{
    public async Task<Result<SpeciesBreedDto, ErrorList>> GetSpeciesBreedsInfoByIds(
        GetSpeciesBreedByIdsDto dto, CancellationToken cancellationToken = default)
    {
        var query = new GetSpeciesBreedsByIds.GetSpeciesBreedsByIdsQuery(dto);
        
        var result = await getSpeciesBreedsInfoHandler.Handle(query, cancellationToken);
        
        return result;
    }

    public async Task<Result<IReadOnlyList<SpeciesBreedDto>, ErrorList>> GetSpeciesBreedsInfoListByIds(
        IEnumerable<GetSpeciesBreedByIdsDto> dtos, CancellationToken cancellationToken = default)
    {
        var query = new GetSpeciesBreedsListByIds.GetSpeciesBreedsListByIdsQuery(dtos);
        
        var result = await getListSpeciesBreedsInfoHandler.Handle(query, cancellationToken);
        
        return result;
    }

    public async Task<UnitResult<ErrorList>> CheckExistSpeciesBreedsByIds(
        GetSpeciesBreedByIdsDto dto, CancellationToken cancellationToken = default)
    {
        var query = new CheckExistSpeciesBreedByIds.CheckExistSpeciesBreedByIdsQuery(dto);
        
        var result = await checkExistHandler.Handle(query, cancellationToken);
        
        return result;
    }
}