/*using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddMainFile;
using PetFamily.Infrastructure.Extensions;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Pets;

public class AddMainFileTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    private const string BUCKET_NAME = "photos";
    
    [Fact]
    public async Task AddMainFile_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var pet = await CreatePet("test-pet", cancellationToken);
        
        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);
        
        var fileName = Guid.NewGuid().ToString();
        
        var fileContent = "Test file content";
        
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        
        var fileDto = new FileDto(stream, fileName);
        
        var command = new AddPetFileCommand(volunteerForPet.Id.Value, pet.Id.Value, [fileDto]);

        //act
        var result = await ExecuteHandlers<AddMainFileHandler, Result<FilePath, ErrorList>>(
            async sut => await sut.Handler(command, cancellationToken));

        //assert
        Assert.True(result.IsSuccess);

        var mainFile = await ExecuteInDatabase(async context =>
        {
            var existPet = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
            
            return existPet?.MainFile;
        });
        
        Assert.NotNull(mainFile);

        var fileStat = await ExecuteInMinio(async client =>
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(mainFile.Value)
                .WithCallbackStream(_ => { });

            return await client.CheckObjectAsync(getObjectArgs, cancellationToken);
        });
        
        Assert.True(fileStat.IsSuccess);
    }
    
    [Fact]
    public async Task AddMainFile_WithEmptyFileName_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var pet = await CreatePet("test-pet", cancellationToken);
        
        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);
        
        var fileName = string.Empty;
        var fileContent = "Test file content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        
        var fileDto = new FileDto(stream, fileName);
        
        var command = new AddPetFileCommand(volunteerForPet.Id.Value, pet.Id.Value, [fileDto]);

        //act
        var result = await ExecuteHandlers<AddMainFileHandler, Result<FilePath, ErrorList>>(
            async sut => await sut.Handler(command, cancellationToken));

        //assert
        Assert.True(result.IsFailure);

        var mainFile = await ExecuteInDatabase(async context =>
        {
            var existPet = await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
            
            return existPet?.MainFile;
        });
        
        Assert.Null(mainFile!.Value);

        var fileStat = await ExecuteInMinio(async client =>
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(mainFile!.Value)
                .WithCallbackStream(_ => { });

            return await client.CheckObjectAsync(getObjectArgs, cancellationToken);
        });
        
        Assert.True(fileStat.IsFailure);
    }
}*/