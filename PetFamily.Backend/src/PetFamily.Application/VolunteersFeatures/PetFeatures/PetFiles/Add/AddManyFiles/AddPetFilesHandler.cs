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

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;

public class AddPetFilesHandler(
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IMinioBucketOptions bucketOptions,
    IValidator<AddPetFileCommand> validator,
    IMassageChannel<IEnumerable<FileData>> massageChannel,
    ILogger<AddPetFilesHandler> logger)
{
    public async Task<Result<IReadOnlyList<FilePath>, ErrorList>> Handle(
        AddPetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        
        var petExist = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == petId);
        if (petExist is null)
            return Errors.Pet.NotFound().ToErrorList();

        List<StreamFileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);

            var fileContent = new StreamFileData(file.Stream, new FileData(filePath, bucketOptions.BucketPhotos));
            filesData.Add(fileContent);
        }
        
        var result = await fileProvider.UploadFiles(filesData, cancellationToken);
        if (result.IsFailure)
        {
            // запись данных о путях в Channel
            await massageChannel.WriteAsync(filesData.Select(f => f.FileData), cancellationToken);
            
            return result.Error;
        }
        
        foreach (var file in filesData)
        {
            petExist.AddPhoto(file.FileData.FilePath);
            
            logger.LogInformation("Added file {fileName}", file.FileData.FilePath.Value);
        }

        var saveResult = await volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        if (saveResult.IsFailure)
        {
            // запись данных о путях в Channel
            await massageChannel.WriteAsync(filesData.Select(f => f.FileData), cancellationToken);
            
            return saveResult.Error.ToErrorList();
        }

        return result;
    }
}