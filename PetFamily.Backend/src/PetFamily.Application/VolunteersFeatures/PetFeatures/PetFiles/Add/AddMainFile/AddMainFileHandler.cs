using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Options;
using PetFamily.Application.Providers;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddMainFile;

public class AddMainFileHandler(
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IMinioBucketOptions bucketOptions,
    IValidator<AddPetFileCommand> validator,
    IMassageChannel<FileData> massageChannel,
    ILogger<AddMainFileHandler> logger)
{
    public async Task<Result<FilePath, ErrorList>> Handler(
        AddPetFileCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerExistResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerExistResult.IsFailure)
            return volunteerExistResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);

        var petExistResult = volunteerExistResult.Value.GetPetById(petId);
        if (petExistResult.IsFailure)
            return petExistResult.Error.ToErrorList();

        var fileDto = command.Files.First();

        var extension = Path.GetExtension(fileDto.FileName);

        var filePath = FilePath.Create(Guid.NewGuid(), extension);

        var fileContent = new StreamFileData(
            fileDto.Stream, 
            new FileData(filePath, bucketOptions.BucketPhotos));

        var result = await fileProvider.UploadFiles([fileContent], cancellationToken);
        if (result.IsFailure)
        {
            // запись данных о пути в Channel
            await massageChannel.WriteAsync(fileContent.FileData, cancellationToken);
            
            return result.Error;
        }

        var mainFile = MainFile.Create(filePath.Value);
        if (mainFile.IsFailure)
        {
            // запись данных о пути в Channel
            await massageChannel.WriteAsync(fileContent.FileData, cancellationToken);
            
            return mainFile.Error.ToErrorList();
        }

        petExistResult.Value.SetMainPhoto(mainFile.Value);
        
        var saveResult = await volunteersRepository.Save(volunteerExistResult.Value, cancellationToken);
        if (saveResult.IsFailure)
        {
            // запись данных о пути в Channel
            await massageChannel.WriteAsync(fileContent.FileData, cancellationToken);
            
            return saveResult.Error.ToErrorList();
        }
        
        logger.LogInformation("Added main file for pet with id {petId}", petId.Value);

        return result.Value[0];
    }
}