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
using rescute.Domain.Aggregates.TimelineEvents;

namespace rescute.Tests.APITests
{

    [Collection("Database collection")]
    public class TimelineEventsControllerTests
    {
        private readonly IConfiguration config = new Configuration();
        [Fact]
        public async System.Threading.Tasks.Task TimelineEventsControllerGetsTimlineEvent()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Test1"), new Name("Test2"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    
                    var animal = new Animal(DateTime.Now, samaritan.Id, $"Test description", AnimalType.Cat());
                    uoW.Samaritans.Add(samaritan);
                    uoW.Animals.Add(animal);
                    var timelineEvent = new StatusReported(DateTime.Now, samaritan.Id, animal.Id, "He's not feeling good",)
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineEventsController(storageService, uoW, config);

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
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
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
