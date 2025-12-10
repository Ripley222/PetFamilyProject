using Application.Abstraction;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Application.VolunteersFeatures.Update.Requisites;

public record UpdateRequisitesCommand(
    Guid VolunteerId,
    IEnumerable<RequisitesDto> Requisites) : ICommand;