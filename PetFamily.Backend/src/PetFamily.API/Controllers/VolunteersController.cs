using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Requests.Volunteer.Create;
using PetFamily.API.Requests.Volunteer.Get;
using PetFamily.API.Requests.Volunteer.Pet;
using PetFamily.API.Requests.Volunteer.Update;
using PetFamily.API.Response;
using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.Delete;
using PetFamily.Application.VolunteersFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.GetById;
using PetFamily.Application.VolunteersFeatures.GetWithPagination;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Move;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddMainFile;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;
using PetFamily.Application.VolunteersFeatures.Update.MainInfo;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;
using PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

namespace PetFamily.API.Controllers;

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

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
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
        var formFileProcessor = new FormFileProcessor();
        var fileDto = formFileProcessor.Process(file);

        var command = new AddPetFileCommand(volunteerId, petId, fileDto);

        var result = await handler.Handler(command, cancellationToken);
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
        var formFileProcessor = new FormFileProcessor();
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

        var envelope = Envelope.Ok(result);

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