using System.Text.Json.Serialization;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HelpStatusEnum
{
    NeedsHelp,
    LookingHome,
    FoundHome
}