using Volunteers.Application.VolunteersFeatures.Update.MainInfo;

namespace Volunteers.Presenters.Requests.Volunteer.Update;

public record UpdateVolunteerMainInfoRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    string EmailAddress,
    string Description,
    int YearsOfExperience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            FirstName,
            MiddleName,
            LastName,
            EmailAddress,
            Description,
            YearsOfExperience,
            PhoneNumber);
}