using Application.Abstraction;
using Application.Extensions;
using Application.Messaging;
using Application.Options;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.FileProvider;
using Volunteers.Application.Providers;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;

public class AddPetFilesHandler(
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IMinioBucketOptions bucketOptions,
    IValidator<AddPetFileCommand> validator,
    IMassageChannel<IEnumerable<FileData>> massageChannel,
    ILogger<AddPetFilesHandler> logger) : ICommandHandler<IReadOnlyList<FilePath>, AddPetFileCommand>
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
            return Errors.PetErrors.NotFound().ToErrorList();

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