using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.SpeciesFeatures.BreedsFeatures.Delete;
using PetFamily.Application.SpeciesFeatures.BreedsFeatures.Get;
using PetFamily.Application.SpeciesFeatures.Delete;
using PetFamily.Application.SpeciesFeatures.Get;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] GetSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }

    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<IActionResult> GetBreedBySpeciesId(
        [FromRoute] Guid speciesId,
        [FromServices] GetBreedsBySpeciesIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new GetBreedsByIdSpeciesQuery(speciesId),
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }
    
    [HttpDelete("{speciesId:guid}")]
    public async Task<IActionResult> DeleteSpecies(
        [FromRoute] Guid speciesId,
        [FromServices] DeleteSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DeleteSpeciesCommand(speciesId),
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }

    [HttpDelete("breeds/{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(
        [FromRoute] Guid breedId,
        [FromServices] DeleteBreedsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DeleteBreedsCommand(breedId),
            cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }
}