using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Update.Requisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites);