using CSharpFunctionalExtensions;
using Minio.DataModel.Args;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class GetFileLinkTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    private const string BUCKET_NAME = "photos";
    
    [Fact]
    public async Task GetFileLink_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var fileName = Guid.NewGuid();
        var fileContent = "Test file content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

        var pet = await CreatePet("test-pet", cancellationToken);
        pet.AddPhoto(FilePath.Create(fileName, ""));

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

        var command = new GetPetFileLinkCommand(volunteerForPet.Id.Value, pet.Id.Value, fileName.ToString());

        //act
        var getLinkResult = await ExecuteHandlers<GetPetFileLinkHandler, Result<string, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(getLinkResult.IsSuccess);

        Assert.NotNull(getLinkResult.Value);
        Assert.NotEmpty(getLinkResult.Value);
    }
    
    [Fact]
    public async Task GetFileLink_NonExistentFile_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var fileName = Guid.NewGuid();
        var fileContent = "Test file content";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

        var pet = await CreatePet("test-pet", cancellationToken);
        pet.AddPhoto(FilePath.Create(fileName, ""));

        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);

        var command = new GetPetFileLinkCommand(volunteerForPet.Id.Value, pet.Id.Value, fileName.ToString());

        //act
        var getLinkResult = await ExecuteHandlers<GetPetFileLinkHandler, Result<string, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(getLinkResult.IsFailure);
    }
}