using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerRequisitesRequest(
    IEnumerable<RequisitesDto> Requisites);