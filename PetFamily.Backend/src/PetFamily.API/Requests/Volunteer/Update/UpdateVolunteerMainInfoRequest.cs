namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerMainInfoRequest(
    string FirstName, 
    string MiddleName,
    string LastName, 
    string EmailAddress, 
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber);