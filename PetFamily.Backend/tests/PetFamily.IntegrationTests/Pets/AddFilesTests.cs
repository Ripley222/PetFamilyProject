using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;
using PetFamily.IntegrationTests.Handlers;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class AddFilesTests(WebTestsFactory testsFactory) : ExecutePetsHandlers(testsFactory), IAsyncDisposable
{
    private readonly List<FileDto> _filesDto = [];

    private const string BUCKET_NAME = "photos";
    
    [Fact]
    public async Task AddFiles_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteer = await CreateVolunteer([pet], cancellationToken);

        for (int i = 0; i < 3; i++)
        {
            var fileContent = "Test file content";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            
            var fileDto = new FileDto(stream, Guid.NewGuid().ToString());
            _filesDto.Add(fileDto);
        }

        var command = new AddPetFileCommand(volunteer.Id.Value, pet.Id.Value, _filesDto);

        //act
        var addFilesResult = await ExecuteAddFilesHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(addFilesResult.IsSuccess);

        var filesInDb = await ExecuteInDatabase(async context =>
        {
            var existPet = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);

            return existPet?.Files;
        });
        
        Assert.NotNull(filesInDb);
        Assert.Equal(_filesDto.Count, filesInDb.Count);

        var filesInMinio = await ExecuteInMinio(async client =>
        {
            var listObjectsArgs = new ListObjectsArgs()
                .WithBucket(BUCKET_NAME);

            return await client.ListObjectsAsync(listObjectsArgs, cancellationToken);
        });
        
        Assert.NotNull(filesInMinio);
    }
    
    [Fact]
    public async Task AddFiles_WithEmptyFilesList_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);

        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);

        var command = new AddPetFileCommand(volunteerForPet.Id.Value, pet.Id.Value, []);

        //act
        var addFilesResult = await ExecuteAddFilesHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(addFilesResult.IsFailure);

        var filesInDb = await ExecuteInDatabase(async context =>
        {
            var existPet = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);

            return existPet?.Files;
        });

        Assert.Empty(filesInDb!);

        var bucketWithFilesInMinio = await ExecuteInMinio(async client =>
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(BUCKET_NAME);
            
            return await client.BucketExistsAsync(bucketExistArgs, cancellationToken);
        });
        
        Assert.False(bucketWithFilesInMinio);
    }

    public new async ValueTask DisposeAsync()
    {
        foreach (var fileDto in _filesDto)
        {
            await fileDto.Stream.DisposeAsync();
        }
        
        GC.SuppressFinalize(this);
    }
}