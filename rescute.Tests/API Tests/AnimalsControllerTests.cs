using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using rescute.API.Controllers;
using rescute.API.Extensions;
using rescute.API.Models;
using rescute.Infrastructure.Services;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using rescute.Tests.InfrastructureTests;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Threading.Tasks;

namespace rescute.Tests.APITests
{

    [Collection("Database collection")]
    public class AnimalsControllerTests
    {
        private readonly IConfiguration config = new Configuration();
        [Fact]
        public async Task AnimalControllerPostsAnimal()
        {
            // Arrange
            using (var context = new rescuteContext(TestUtility.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var animalDesc = "my new animal";
                    var attachments = FileStorageServiceTests.CreateIFormFileAttachments(3, 4, 5, "descritpion for files","image/jpg");

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new AnimalsController(storageService, uoW, config);
                    var postModel = new AnimalPostModel() { AnimalType = AnimalType.Pigeon().ToString(), Description = animalDesc, Attachments = attachments };
                    // Act

                    var result = await controller.PostAnimal(postModel);


                    // Assert
                    result.Should().NotBeNull();                    
                    var created = result as CreatedAtActionResult;
                    var getModel =  created.Value as AnimalGetModel;
                    getModel.Attachments.Count().Should().Be(3);
                    getModel.AnimalType.Should().Be(AnimalType.Pigeon().ToString());
                    getModel.Description.Should().Be(animalDesc);
                }
            }
        }
                [Fact]
        public async Task AnimalControllerGetsAnimal()
        {
            // Arrange
            using (var context = new rescuteContext(TestUtility.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Test1"), new Name("Test2"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    uoW.Samaritans.Add(samaritan);
                    var animal = new Animal(DateTime.Now, samaritan.Id, $"Test description", AnimalType.Pigeon());
                    uoW.Animals.Add(animal);
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new AnimalsController(storageService, uoW, config);

                    // Act

                    var result = await controller.Get(animal.Id.Value);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var stored = ((AnimalGetModel)((OkObjectResult)result.Result).Value);
                    stored.AnimalId.Should().Be(animal.Id.Value);
                }
            }

        }

        [Fact]
        public async System.Threading.Tasks.Task AnimalControllerGetsPagedAnimals()
        {
            // Arrange
            using (var context = new rescuteContext(TestUtility.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Test1"), new Name("Test2"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    uoW.Samaritans.Add(samaritan);

                    for (int i = 1; i <= 10; i++)
                    {
                        uoW.Animals.Add(new Animal(DateTime.Now, samaritan.Id, $"Test description {i}", AnimalType.Dog()));
                    }
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new AnimalsController(storageService, uoW, config);

                    // Act
                    var result = await controller.Get(10, 0);

                    // Assert

                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var animalModels = ((IEnumerable<AnimalGetModel>)((OkObjectResult)result.Result).Value);
                    animalModels.Count().Should().Be(10);

                }
            }
        }
    }
}
