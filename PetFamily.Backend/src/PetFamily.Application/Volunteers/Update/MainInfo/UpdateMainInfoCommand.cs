namespace PetFamily.Application.Volunteers.Update.MainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    string FirstName, 
    string MiddleName,
    string LastName, 
    string EmailAddress, 
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber);