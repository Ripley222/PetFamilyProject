using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;

public class AddPetFileHandler(
    IVolunteersRepository repository,
    IFileProvider fileProvider,
    IValidator<AddPetFileCommand> validator,
    IMassageChannel<IEnumerable<FileData>>  massageChannel,
    ILogger<AddPetFileHandler> logger)
{
    private const string BUCKET_NAME = "photos";
    
    public async Task<Result<IReadOnlyList<FilePath>, ErrorList>> Handle(
        AddPetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerResult = await repository.GetById(
            VolunteerId.Create(command.VolunteerId), cancellationToken);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petExist = volunteerResult.Value.Pets.FirstOrDefault(
            p => p.Id == PetId.Create(command.PetId));

        if (petExist is null)
            return Errors.Pet.NotFound().ToErrorList();

        List<StreamFileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileContent = new StreamFileData(file.Stream, new FileData(filePath.Value, BUCKET_NAME));
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

        await repository.Save(volunteerResult.Value, cancellationToken);
        
        return result;
    }
}