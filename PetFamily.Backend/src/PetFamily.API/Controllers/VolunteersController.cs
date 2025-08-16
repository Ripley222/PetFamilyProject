using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Requests.Volunteer.Create;
using PetFamily.API.Requests.Volunteer.Pet;
using PetFamily.API.Requests.Volunteer.Update;
using PetFamily.API.Response;
using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.Delete;
using PetFamily.Application.VolunteersFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Move;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using PetFamily.Application.VolunteersFeatures.Update.MainInfo;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;
using PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerCommand(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.EmailAddress,
            request.Description,
            request.YearsOfExperience,
            request.PhoneNumber,
            request.Requisites,
            request.SocialNetworks);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMainInfoCommand(
            id,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.EmailAddress,
            request.Description,
            request.YearsOfExperience,
            request.PhoneNumber);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{id:guid}/social-networks-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworkRequest request,
        [FromServices] UpdateSocialNetworksHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSocialNetworksCommand(id, request.SocialNetworks);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{id:guid}/requisites-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequisitesRequest request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRequisitesCommand(id, request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpDelete("{id:guid}/soft-delete")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpDelete("{id:guid}/hard-delete")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddPetCommand(
            volunteerId,
            request.SpeciesName,
            request.BreedName,
            request.PetName,
            request.Description,
            request.Color,
            request.HealthInformation,
            request.City,
            request.Street,
            request.House,
            request.Weight,
            request.Height,
            request.PhoneNumber,
            request.IsNeutered,
            request.DateOfBirth,
            request.IsVaccinated,
            request.HelpStatus,
            request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photo")]
    public async Task<ActionResult<Guid>> Add(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IFormFileCollection files,
        [FromServices] AddPetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var formFileProcessor = new FormFileProcessor();
        var filesDto = formFileProcessor.Process(files);

        var command = new AddPetFileCommand(volunteerId, petId, filesDto);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpGet("{volunteerId:guid}/pet/{petId:guid}/photo-link")]
    public async Task<ActionResult<Guid>> Get(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] GetPetFileLinkRequest request,
        [FromServices] GetPetFileLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GetPetFileLinkCommand(
            volunteerId,
            petId,
            request.FileName);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/pet-photo")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] DeletePetFileRequest request,
        [FromServices] DeletePetFileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetFileCommand(
            volunteerId, 
            petId,
            request.FileName);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/move")]
    public async Task<ActionResult<Guid>> MovePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery]  MovePetRequest request,
        [FromServices] MovePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new MovePetCommand(
            volunteerId,
            petId,
            request.NewPosition);
        
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }
}