using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests.CreateVolunteer;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.DTOs;

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
        
        var result = await handler.Handler(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        var envelope = Envelope.Ok(result.Value);
        
        return Ok(envelope);
    }
}