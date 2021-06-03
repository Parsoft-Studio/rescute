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
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Tests.APITests
{

    [Collection("Database collection")]
    public class TimelineItemsControllerTests
    {
        private readonly IConfiguration config = new Configuration();
        [Fact]
        public async Task TimelineItemsControllerPostsStatusReport()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var eventDesc = "He is doing fine.";
                    var location = new MapPoint(2, 3);

                    var samaritan = Samaritan.RandomTestSamaritan();
                    var animal = Animal.RandomTestAnimal(samaritan.Id);

                    uoW.Samaritans.Add(samaritan);
                    uoW.Animals.Add(animal);
                    await uoW.Complete();

                    var attachments = FileStorageServiceTests.CreateIFormFileAttachments(3, 4, 5, "descritpion for files", "image/png");

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);
                    var postModel = new StatusReportPostModel() { Attachments = attachments, AnimalId = animal.Id.ToString(), Description = eventDesc, Lattitude = location.Latitude, Longitude = location.Longitude };


                    // Act

                    var result = await controller.PostStatus(postModel);


                    // Assert
                    result.Should().NotBeNull();
                    var created = result as CreatedAtActionResult;
                    var getModel = created.Value as StatusReportGetModel;
                    getModel.Attachments.Count().Should().Be(3);
                    getModel.AnimalId.Should().Be(animal.Id.ToString());
                    getModel.Lattitude.Should().Be(location.Latitude);
                    getModel.Longitude.Should().Be(location.Longitude);
                    getModel.Description.Should().Be(eventDesc);
                }
            }
        }
        [Fact]
        public async Task TimelineItemsControllerGetsStatusReport()
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
                    var animal = Animal.RandomTestAnimal(samaritan.Id);
                    uoW.Animals.Add(animal);
                    var attachment = new Attachment("image.jpg", "jpg", DateTime.Now, "desc");
                    var statusEvent = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, location, eventDesc, attachment);
                    uoW.TimelineItems.Add(statusEvent);

                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act

                    var result = await controller.GetStatus(statusEvent.Id.Value);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var stored = ((StatusReportGetModel)((OkObjectResult)result.Result).Value);
                    stored.EventId.Should().Be(statusEvent.Id.ToString());
                    stored.Attachments.Count().Should().Be(1);
                    stored.Description.Should().Be(eventDesc);
                    stored.CreatedById.Should().Be(samaritan.Id.ToString());

                }
            }

        }

        [Fact]
        public async Task TimelineItemsControllerGetsPagedStatusReports()
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

                        uoW.TimelineItems.Add(new StatusReport(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), $"Test description {i}"));
                    }
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act
                    var result = await controller.GetStatus(10, 0);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var statusModels = ((IEnumerable<StatusReportGetModel>)((OkObjectResult)result.Result).Value);
                    statusModels.Count().Should().Be(10);

                }
            }
        }

        [Fact]
        public async Task TimelineItemsControllerRequestsTransport()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var eventDesc = "Want to take him to this hospital.";
                    var location = new MapPoint(2, 3);
                    var toLocation = new MapPoint(3, 4);

                    var samaritan = Samaritan.RandomTestSamaritan();
                    var animal = Animal.RandomTestAnimal(samaritan.Id);

                    uoW.Samaritans.Add(samaritan);
                    uoW.Animals.Add(animal);
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);
                    var postModel = new TransportRequestPostModel()
                    {
                        AnimalId = animal.Id.ToString(),
                        Description = eventDesc,
                        Lattitude = location.Latitude,
                        Longitude = location.Longitude,
                        ToLattitude = toLocation.Latitude,
                        ToLongitude = toLocation.Longitude
                    };


                    // Act

                    var result = await controller.PostTransportRequest(postModel);


                    // Assert
                    result.Should().NotBeNull();
                    var created = result as CreatedAtActionResult;
                    var getModel = created.Value as TransportRequestGetModel;

                    getModel.AnimalId.Should().Be(animal.Id.ToString());
                    getModel.Lattitude.Should().Be(location.Latitude);
                    getModel.Longitude.Should().Be(location.Longitude);
                    getModel.ToLattitude.Should().Be(toLocation.Latitude);
                    getModel.ToLongitude.Should().Be(toLocation.Longitude);
                    getModel.Description.Should().Be(eventDesc);
                }
            }
        }
        [Fact]
        public async Task TimelineItemsControllerGetsTransportRequest()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var location = new MapPoint(20, 30);
                    var toLocation = new MapPoint(30, 40);
                    var eventDesc = "I want to take it there.";
                    var samaritan = Samaritan.RandomTestSamaritan();
                    uoW.Samaritans.Add(samaritan);
                    var animal = Animal.RandomTestAnimal(samaritan.Id);
                    uoW.Animals.Add(animal);
                    var transportEvent = new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, location, toLocation, eventDesc);
                    uoW.TimelineItems.Add(transportEvent);

                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act

                    var result = await controller.GetTransportRequest(transportEvent.Id.Value);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var stored = ((TransportRequestGetModel)((OkObjectResult)result.Result).Value);
                    stored.EventId.Should().Be(transportEvent.Id.ToString());
                    stored.Description.Should().Be(eventDesc);
                    stored.CreatedById.Should().Be(samaritan.Id.ToString());
                    stored.AnimalId.Should().Be(animal.Id.ToString());
                    stored.Lattitude.Should().Be(location.Latitude);
                    stored.Longitude.Should().Be(location.Longitude);
                    stored.ToLattitude.Should().Be(toLocation.Latitude);
                    stored.ToLongitude.Should().Be(toLocation.Longitude);

                }
            }

        }

        [Fact]
        public async Task TimelineItemsControllerGetsPagedTransportRequests()
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

                        uoW.TimelineItems.Add(new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), new MapPoint(20, 30), $"Test description {i}"));
                    }
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act
                    var result = await controller.GetTransportRequests(10, 0);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var statusModels = ((IEnumerable<TransportRequestGetModel>)((OkObjectResult)result.Result).Value);
                    statusModels.Count().Should().Be(10);
                }
            }
        }

        //[Theory]
        //[InlineData(true, false, false)]
        //[InlineData(false, true, false)]
        //[InlineData(false, false, true)]
        //public async Task TimelineItemsControllerPostsBill(bool includesLab, bool includesPrescription, bool includesVetFee)
        //{
        //    // Arrange
        //    using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
        //    {
        //        using (var uoW = new UnitOfWork(context))
        //        {
        //            var eventDesc = "This is the bill I got from the vet.";

        //            var samaritan = Samaritan.RandomTestSamaritan();
        //            var animal = Animal.RandomTestAnimal(samaritan.Id);
        //            var attachments = FileStorageServiceTests.CreateIFormFileAttachments(1, 1, 1, "descritpion for files", "image/png");

        //            var medDoc1 = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, string.Empty, MedicalDocumentType.DoctorsOrders(), new Attachment("filename", "jpg", DateTime.Now, string.Empty));
        //            var medDoc2 = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, string.Empty, MedicalDocumentType.LabResults(), new Attachment("filename", "jpg", DateTime.Now, string.Empty));
        //            var medDoc3 = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, string.Empty, MedicalDocumentType.Prescription(), new Attachment("filename", "jpg", DateTime.Now, string.Empty));

        //            decimal billTotal = 1000;
        //            uoW.Samaritans.Add(samaritan);
        //            uoW.Animals.Add(animal);
        //            uoW.TimelineItems.Add(medDoc1);
        //            uoW.TimelineItems.Add(medDoc2);
        //            uoW.TimelineItems.Add(medDoc3);
        //            await uoW.Complete();



        //            var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
        //            var controller = new TimelineItemsController(storageService, uoW, config);
        //            var postModel = new BillPostModel()
        //            {
        //                AnimalId = animal.Id.ToString(),
        //                Description = eventDesc,
        //                Attachments = attachments,
        //                IncludesLabResults = includesLab,
        //                IncludesPrescription = includesPrescription,
        //                IncludesVetFee = includesVetFee,
        //                Total = billTotal,
        //                RelatedMedicalDocumentIds = new List<string>() { medDoc1.Id.ToString(), medDoc2.Id.ToString(), medDoc3.Id.ToString() }
        //            };


        //            // Act

        //            var result = await controller.PostBill(postModel);


        //            // Assert
        //            result.Should().NotBeNull();
        //            var created = result as CreatedAtActionResult;
        //            var getModel = created.Value as BillGetModel;

        //            getModel.AnimalId.Should().Be(animal.Id.ToString());
        //            getModel.Description.Should().Be(eventDesc);
        //            getModel.Attachments.Count().Should().Be(1);
        //            getModel.IncludesVetFee.Should().Be(includesVetFee);
        //            getModel.IncludesLabResults.Should().Be(includesLab);
        //            getModel.IncludesPrescription.Should().Be(includesPrescription);
        //            getModel.Total.Should().Be(billTotal);
        //            getModel.RelatedMedicalDocumentIds.Count().Should().Be(3);
        //        }
        //    }
        //}
        [Fact]
        public async Task TimelineItemsControllerGetsBill()
        {
            // Arrange
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var uoW = new UnitOfWork(context))
                {
                    var location = new MapPoint(20, 30);
                    var toLocation = new MapPoint(30, 40);
                    var eventDesc = "I want to take it there.";
                    var samaritan = Samaritan.RandomTestSamaritan();
                    uoW.Samaritans.Add(samaritan);
                    var animal = Animal.RandomTestAnimal(samaritan.Id);
                    uoW.Animals.Add(animal);
                    var transportEvent = new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, location, toLocation, eventDesc);
                    uoW.TimelineItems.Add(transportEvent);

                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act

                    var result = await controller.GetTransportRequest(transportEvent.Id.Value);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var stored = ((TransportRequestGetModel)((OkObjectResult)result.Result).Value);
                    stored.EventId.Should().Be(transportEvent.Id.ToString());
                    stored.Description.Should().Be(eventDesc);
                    stored.CreatedById.Should().Be(samaritan.Id.ToString());
                    stored.AnimalId.Should().Be(animal.Id.ToString());
                    stored.Lattitude.Should().Be(location.Latitude);
                    stored.Longitude.Should().Be(location.Longitude);
                    stored.ToLattitude.Should().Be(toLocation.Latitude);
                    stored.ToLongitude.Should().Be(toLocation.Longitude);

                }
            }

        }

        [Fact]
        public async Task TimelineItemsControllerGetsPagedBills()
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

                        uoW.TimelineItems.Add(new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), new MapPoint(20, 30), $"Test description {i}"));
                    }
                    await uoW.Complete();

                    var storageService = new FileStorageService(FileStorageServiceTests.TestFileStorageRoot, new List<string>() { "jpg", "avi", "png", "mp4" });
                    var controller = new TimelineItemsController(storageService, uoW, config);

                    // Act
                    var result = await controller.GetTransportRequests(10, 0);

                    // Assert
                    result.Should().NotBeNull();
                    result.Result.Should().BeOfType<OkObjectResult>();

                    ((OkObjectResult)result.Result).Value.Should().NotBeNull();
                    var statusModels = ((IEnumerable<TransportRequestGetModel>)((OkObjectResult)result.Result).Value);
                    statusModels.Count().Should().Be(10);
                }
            }
        }

    }
}
