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
using rescute.Domain.Aggregates.TimelineEvents;

namespace rescute.Tests.APITests
{

    [Collection("Database collection")]
    public class TimelineEventsControllerTests
    {
        private readonly IConfiguration config = new Configuration();
        [Fact]
        public async Task TimelineEventsControllerPostsStatusReport()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var eventDesc = "He is doing fine.";
                    var location = new MapPoint(2, 3);

                    var samaritan = new Samaritan(new Name("Test1"), new Name("Test2"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    var animal = new Animal(DateTime.Now, samaritan.Id, $"Test description", AnimalType.Pigeon());

                    uoW.Samaritans.Add(samaritan);
                    uoW.Animals.Add(animal);
                    await uoW.Complete();

                    var attachments = FileStorageServiceTests.CreateIFormFileAttachments(3, 4, 5, "descritpion for files", "image/png");

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineEventsController(storageService, uoW, config);
                    var postModel = new StatusReportedPostModel() { Attachments = attachments, AnimalId = animal.Id.ToString(), Description = eventDesc, Lattitude = location.Latitude, Longitude = location.Longitude };


                    // Act

                    var result = await controller.PostStatus(postModel);


                    // Assert
                    result.Should().NotBeNull();
                    var created = result as CreatedAtActionResult;
                    var getModel = created.Value as StatusReportedGetModel;
                    getModel.Attachments.Count().Should().Be(3);
                    getModel.AnimalId.Should().Be(animal.Id.ToString());
                    getModel.Lattitude.Should().Be(location.Latitude);
                    getModel.Longitude.Should().Be(location.Longitude);
                    getModel.Description.Should().Be(eventDesc);
                }
            }
        }
        [Fact]
        public async Task TimelineEventsControllerGetsStatusReportedEvent()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var location = new MapPoint(20, 30);
                    var eventDesc = "Something went wrong.";
                    var samaritan = Samaritan.RandomTestSamaritan();
                    uoW.Samaritans.Add(samaritan);
                    var animal = new Animal(DateTime.Now, samaritan.Id, $"Test description", AnimalType.Pigeon());
                    uoW.Animals.Add(animal);
                    var attachment = new Attachment(AttachmentType.Image(), "image.jpg", DateTime.Now, "desc");
                    var statusEvent = new StatusReported(DateTime.Now, samaritan.Id, animal.Id, location, eventDesc,attachment);
                    uoW.TimelineEvents.Add(statusEvent);

                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });                    
                    var controller = new TimelineEventsController(storageService, uoW, config);

                    // Act

                    var result = await controller.GetStatus(statusEvent.Id.Value);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var stored = ((StatusReportedGetModel)((OkObjectResult)result.Result).Value);
                    stored.EventId.Should().Be(statusEvent.Id.ToString());
                    stored.Attachments.Count().Should().Be(1);
                    stored.Description.Should().Be(eventDesc);
                    stored.CreatedById.Should().Be(samaritan.Id.ToString());
                    
                }
            }

        }

        [Fact]
        public async Task TimelineEventsControllerGetsPagedStatusReportedEvents()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var samaritan = Samaritan.RandomTestSamaritan();
                    var animal = Animal.RandomTestAnimal(samaritan.Id);
                    
                    uoW.Animals.Add(animal);
                    uoW.Samaritans.Add(samaritan);

                    for (int i = 1; i <= 10; i++)
                    {
                        
                        uoW.TimelineEvents.Add(new StatusReported(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), $"Test description {i}"));
                    }
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineEventsController(storageService, uoW, config);

                    // Act
                    var result = await controller.GetStatus(10, 0);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var statusModels = ((IEnumerable<StatusReportedGetModel>)((OkObjectResult)result.Result).Value);
                    statusModels.Count().Should().Be(10);

                }
            }
        }
    }
}
