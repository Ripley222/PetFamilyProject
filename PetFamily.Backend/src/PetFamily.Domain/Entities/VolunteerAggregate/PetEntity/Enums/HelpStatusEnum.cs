using System.Text.Json.Serialization;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HelpStatusEnum
{
    NeedsHelp,
    LookingHome,
    FoundHome
}