using System.Text.Json.Serialization;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record RequisitesList
{
    public List<Requisites> Requisites { get; } = [];

    [JsonConstructor]
    public RequisitesList()
    {
    }
    
    public RequisitesList(List<Requisites> requisites)
    {
        Requisites = requisites;
    }
}