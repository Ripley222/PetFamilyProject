using Application.Abstraction;
using CSharpFunctionalExtensions;
using Framework;
using Framework.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Species.Database;
using Species.Domain.SpeciesAggregate;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.SpeciesFeatures.Create;

public sealed class CreateBreeds
{
    public record CreateBreedsCommand(Guid SpeciesId, string BreedName) : ICommand;

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("species/{speciesId:Guid}/breeds", async (
                [FromRoute] Guid speciesId,
                [FromBody] string breedName,
                [FromServices] CreateBreedsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateBreedsCommand(speciesId, breedName);

                var result = await handler.Handle(command, cancellationToken);
                var envelope = Envelope.Ok(result);

                return Results.Ok(envelope);
            });
        }
    }

    public sealed class CreateBreedsHandler(SpeciesDbContext dbContext) : ICommandHandler<Guid, CreateBreedsCommand>
    {
        public async Task<Result<Guid, ErrorList>> Handle(
            CreateBreedsCommand command, CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var breedName = command.BreedName;
            
            var species = await dbContext.Species
                .Include(s => s.Breeds.Where(b => b.Name == breedName))
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);
            
            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();
            
            var breedExists = species.Breeds.FirstOrDefault(b => b.Name == breedName);
            if (breedExists is not null)
                return Errors.BreedsErrors.AlreadyExist().ToErrorList();
            
            var breed = Breed.Create(BreedId.New(), breedName);
            if (breed.IsFailure)
                return breed.Error.ToErrorList();

            species.AddBreed(breed.Value);
            
            await dbContext.SaveChangesAsync(cancellationToken);

            return breed.Value.Id.Value;
        }
    }
}