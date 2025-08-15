namespace PetFamily.Application.VolunteersFeatures.Update.MainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    string FirstName, 
    string MiddleName,
    string LastName, 
    string EmailAddress, 
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber);