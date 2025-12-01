using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Infrastructure.Extensions;
using PetFamily.IntegrationTests.Handlers;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class DeletePetFileTests(WebTestsFactory testsFactory) : ExecutePetsHandlers(testsFactory)
{
    private const string BUCKET_NAME = "photos";
    
    [Fact]
    public async Task DeletePetFile_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var fileName = Guid.NewGuid();
        var fileContent = "Test file content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        
        var pet = await CreatePet("test-pet", cancellationToken);
        pet.AddPhoto(FilePath.Create(fileName, "").Value);
        
        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);

        await ExecuteInMinio(async client =>
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(BUCKET_NAME);

            await client.MakeBucketAsync(makeBucketArgs, cancellationToken);
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(fileName.ToString());

            return await client.PutObjectAsync(putObjectArgs, cancellationToken);
        });
        
        var command = new DeletePetFileCommand(volunteerForPet.Id.Value, pet.Id.Value, fileName.ToString());

        //act
        var removeFileResult = await ExecuteRemoveFileHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(removeFileResult.IsSuccess);

        var petFilesInDb = await ExecuteInDatabase(async context =>
        {
            var petWithRemovingFile = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);

            return petWithRemovingFile?.Files;
        });
        
        Assert.Empty(petFilesInDb!);

        var checkResultPetFilesInMinio = await ExecuteInMinio(async client =>
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName.ToString())
                .WithCallbackStream(_ => { });
            
            return await client.CheckObjectAsync(getObjectArgs, cancellationToken);
        });
        
        Assert.True(checkResultPetFilesInMinio.IsFailure);
    }
    
    [Fact]
    public async Task DeletePetFile_WithInvalidFileName_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var fileName = Guid.NewGuid();
        var fileContent = "Test file content";
        
        var pet = await CreatePet("test-pet", cancellationToken);
        pet.AddPhoto(FilePath.Create(fileName, "").Value);
        
        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);

        await ExecuteInMinio(async client =>
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(BUCKET_NAME);

            await client.MakeBucketAsync(makeBucketArgs, cancellationToken);
            
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(fileName.ToString());

            return await client.PutObjectAsync(putObjectArgs, cancellationToken);
        });
        
        var command = new DeletePetFileCommand(volunteerForPet.Id.Value, pet.Id.Value, string.Empty);

        //act
        var removeFileResult = await ExecuteRemoveFileHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(removeFileResult.IsFailure);

        var petFilesInDb = await ExecuteInDatabase(async context =>
        {
            var petWithRemovingFile = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);

            return petWithRemovingFile?.Files;
        });
        
        Assert.NotEmpty(petFilesInDb!);

        var checkResultPetFilesInMinio = await ExecuteInMinio(async client =>
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName.ToString())
                .WithCallbackStream(_ => { });
            
            return await client.CheckObjectAsync(getObjectArgs, cancellationToken);
        });
        
        Assert.True(checkResultPetFilesInMinio.IsSuccess);
    }
}