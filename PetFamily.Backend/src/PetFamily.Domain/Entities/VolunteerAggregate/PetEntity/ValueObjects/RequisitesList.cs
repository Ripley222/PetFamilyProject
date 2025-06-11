using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record RequisitesList
{
    public IReadOnlyList<Requisites>? Requisites { get; private set; }
}