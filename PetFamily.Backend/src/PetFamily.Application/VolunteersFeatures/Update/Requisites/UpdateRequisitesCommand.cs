using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.Application.VolunteersFeatures.Update.Requisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites);