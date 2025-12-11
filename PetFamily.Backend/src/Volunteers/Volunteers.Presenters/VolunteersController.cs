using Framework;
using Framework.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Application.VolunteersFeatures.Create;
using Volunteers.Application.VolunteersFeatures.Delete;
using Volunteers.Application.VolunteersFeatures.Delete.HardDelete;
using Volunteers.Application.VolunteersFeatures.Delete.SoftDelete;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Application.VolunteersFeatures.GetById;
using Volunteers.Application.VolunteersFeatures.GetWithPagination;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Add;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Delete;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Delete.HardDelete;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Delete.SoftDelete;
using Volunteers.Application.VolunteersFeatures.PetFeatures.GetAll;
using Volunteers.Application.VolunteersFeatures.PetFeatures.GetById;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Move;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddMainFile;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Update.Status;
using Volunteers.Application.VolunteersFeatures.Update.MainInfo;
using Volunteers.Application.VolunteersFeatures.Update.Requisites;
using Volunteers.Application.VolunteersFeatures.Update.SocialNetworks;
using Volunteers.Presenters.Processors;
using Volunteers.Presenters.Requests.Pets;
using Volunteers.Presenters.Requests.Volunteer.Create;
using Volunteers.Presenters.Requests.Volunteer.Get;
using Volunteers.Presenters.Requests.Volunteer.Pet;
using Volunteers.Presenters.Requests.Volunteer.Update;

namespace Volunteers.Presenters;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Guid>> Get(
        [FromQuery] GetVolunteersRequest request,
        [FromServices] GetVolunteersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult<Guid>> GetById(
        [FromRoute] Guid volunteerId,
        [FromServices] GetVolunteersByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetVolunteersByIdQuery(volunteerId);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }

    [HttpGet("{volunteerId:guid}/pets/{petId:guid}/photo-link")]
    public async Task<ActionResult<Guid>> GetPhotoLink(
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

    [HttpGet("pets/{petId:Guid}")]
    public async Task<ActionResult<PetDto>> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetsByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPetsByIdQuery(petId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }
    
    [HttpGet("pets")]
    public async Task<ActionResult<List<PetDto>>> GetPets(
        [FromQuery] GetPetsRequest request,
        [FromServices] GetPetsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/main-photo")]
    public async Task<ActionResult<Guid>> AddPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IFormFile file,
        [FromServices] AddMainFileHandler handler,
        CancellationToken cancellationToken)
    {
        await using var formFileProcessor = new FormFileProcessor();
        var fileDto = formFileProcessor.Process(file);

        var command = new AddPetFileCommand(volunteerId, petId, fileDto);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }

    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IFormFileCollection files,
        [FromServices] AddPetFilesHandler handler,
        CancellationToken cancellationToken)
    {
        await using var formFileProcessor = new FormFileProcessor();
        var filesDto = formFileProcessor.Process(files);

        var command = new AddPetFileCommand(volunteerId, petId, filesDto);

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
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerSocialNetworkRequest request,
        [FromServices] UpdateSocialNetworksHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequisitesRequest request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/move")]
    public async Task<ActionResult<Guid>> MovePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] MovePetRequest request,
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

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdatePetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(volunteerId, petId),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdatePetStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromQuery] UpdatePetStatusRequest request,
        [FromServices] UpdatePetStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(volunteerId, petId),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> SoftDelete(
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

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDelete(
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

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/photo")]
    public async Task<ActionResult<Guid>> DeletePetPhoto(
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

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult<Guid>> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SoftDeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DeletePetCommand(volunteerId, petId),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DeletePetCommand(volunteerId, petId),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);

        return Ok(envelope);
    }
}