/*using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.HardDelete;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Infrastructure.Extensions;
using PetFamily.Infrastructure.Providers;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Pets;

public class HardDeleteTests(WebTestsFactory webTestsFactory) : PetsEntityFactory(webTestsFactory)
{
    private const string BUCKET_NAME = "photos";
    
    [Fact]
    public async Task HardDeletePetWithoutFiles_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("pet", cancellationToken);

        var petsList = new List<Pet> { pet };

        var volunteer = await CreateVolunteer(petsList, cancellationToken);

        var command = new DeletePetCommand(volunteer.Id.Value, pet.Id.Value);

        //act
        var deletingResult = await ExecuteHandlers<HardDeletePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletingResult.IsSuccess);

        var deletingPet = await ExecuteInDatabase(async context =>
        {
            var existVolunteer = await context.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);

            return existVolunteer?.Pets.FirstOrDefault(p => p.Id == pet.Id);
        });

        Assert.Null(deletingPet);
    }

    [Fact]
    public async Task HardDeletePetWithFiles_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var filePath = FilePath.Create(Guid.NewGuid(), ".png");
        
        await ExecuteInMinio(async client =>
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(BUCKET_NAME);

            if (await client.BucketExistsAsync(bucketExistsArgs, cancellationToken) is false)
            {
                var bucketArgs = new MakeBucketArgs()
                    .WithBucket(BUCKET_NAME);

                await client.MakeBucketAsync(bucketArgs, cancellationToken);
            }

            var fileContent = "Hello MinIO!";
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(filePath.Value);

            return await client.PutObjectAsync(putObjectArgs, cancellationToken);
        });

        var pet = await CreatePet("test-pet", cancellationToken);
        pet.AddPhoto(filePath);

        var volunteer = await CreateVolunteer([pet], cancellationToken);

        var command = new DeletePetCommand(volunteer.Id.Value, pet.Id.Value);

        //act
        var deletingResult = await ExecuteHandlers<HardDeletePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletingResult.IsSuccess);

        var deletingPet = await ExecuteInDatabase(async context =>
        {
            var existVolunteer = await context.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);

            return existVolunteer?.Pets.FirstOrDefault(p => p.Id == pet.Id);
        });

        var checkDeletingFileResult = await ExecuteInMinio(async client =>
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(filePath.Value)
                .WithCallbackStream(_ => { });

            return await client.CheckObjectAsync(getObjectArgs, cancellationToken);
        });

        Assert.Null(deletingPet);
        Assert.True(checkDeletingFileResult.IsFailure);
    }
}*/