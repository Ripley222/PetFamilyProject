using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;

public class GetPetFileLinkHandler(
    IVolunteersRepository repository,
    IFileProvider fileProvider,
    IValidator<GetPetFileLinkCommand> validator,
    ILogger<AddPetFileHandler> logger)
{
    private const string BUCKET_NAME = "photos";
    
    public async Task<Result<string, ErrorList>> Handle(
        GetPetFileLinkCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerResult = await repository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petExist = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (petExist is null)
            return Errors.Pet.NotFound().ToErrorList();
        
        var extension = Path.GetExtension(command.FileName);
        var fileName = command.FileName.Replace(extension, string.Empty);
        
        var fileData = new FileData(
            FilePath.Create(Guid.Parse(fileName), extension).Value, 
            BUCKET_NAME);

        var result = await fileProvider.GetFileLink(fileData, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.LogInformation("Getting file link {fileName}", command.FileName);

        return result.Value;
    }
}