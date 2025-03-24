using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;

namespace rescute.Tests;

internal class TestUtility
{
    private TestUtility()
    {
        // utility class
    }

    public static Samaritan RandomTestSamaritan()
    {
        return new Samaritan(new Name("Test First"), new Name("Test Last"), new PhoneNumber(true, "0912121212"),
            DateTime.MinValue);
    }

    public static Animal RandomTestAnimal(Id<Samaritan> introducedBy)
    {
        return new Animal(DateTime.MinValue, introducedBy, "test animal", AnimalType.Cat());
    }

    public static Attachment RandomTestAttachment()
    {
        var filename = Guid.NewGuid().ToString();
        var extension = "jpg";
        var creationDate = DateTime.MinValue;
        var description = "Test attachment";
        return new Attachment(filename, extension, creationDate, description);
    }

    /// <summary>
    /// Gets database options for tests
    /// </summary>
    /// <param name="useDefaultDatabase">
    /// When true, uses the default shared database (for tests that need multiple contexts to access the same data).
    /// When false (default), creates a unique database for test isolation.
    /// </param>
    public static DbContextOptions<rescuteContext> GetTestDatabaseOptions(bool useDefaultDatabase = false)
    {
        return new DbContextOptionsBuilder<rescuteContext>()
            .UseInMemoryDatabase(useDefaultDatabase ? "rescute" : $"rescute_{Guid.NewGuid()}")
            .Options;
    }

    /// <summary>
    /// Creates test data for timeline item repository tests with various filtering scenarios
    /// </summary>
    public static async
        Task<(Samaritan CurrentUser, Samaritan OtherUser, Animal Animal1, Animal Animal2, List<TimelineItem>
            TimelineItems,
            List<Comment> Comments)> CreateTimelineItemTestData(IUnitOfWork unitOfWork)
    {
        // Create two Samaritan users
        var currentUser = RandomTestSamaritan();
        var otherUser = RandomTestSamaritan();

        // Create two animals
        var animal1 = RandomTestAnimal(currentUser.Id);
        var animal2 = RandomTestAnimal(otherUser.Id);

        // Add users and animals to the database
        unitOfWork.Samaritans.Add(currentUser);
        unitOfWork.Samaritans.Add(otherUser);
        unitOfWork.Animals.Add(animal1);
        unitOfWork.Animals.Add(animal2);

        // Create timeline items list to track all created items
        var timelineItems = new List<TimelineItem>();

        // 1. Create status reports from both users for both animals
        var currentUserReport1 = new StatusReport(DateTime.Now, currentUser.Id, animal1.Id,
            new MapPoint(10, 20), "Status report by current user for animal 1",
            RandomTestAttachment());

        var currentUserReport2 = new StatusReport(DateTime.Now, currentUser.Id, animal2.Id,
            new MapPoint(10, 20), "Status report by current user for animal 2",
            RandomTestAttachment());

        var otherUserReport1 = new StatusReport(DateTime.Now, otherUser.Id, animal1.Id,
            new MapPoint(10, 20), "Status report by other user for animal 1",
            RandomTestAttachment());

        var otherUserReport2 = new StatusReport(DateTime.Now, otherUser.Id, animal2.Id,
            new MapPoint(10, 20), "Status report by other user for animal 2",
            RandomTestAttachment());

        // Add status reports to the database and tracking list
        unitOfWork.TimelineItems.Add(currentUserReport1);
        unitOfWork.TimelineItems.Add(currentUserReport2);
        unitOfWork.TimelineItems.Add(otherUserReport1);
        unitOfWork.TimelineItems.Add(otherUserReport2);
        timelineItems.Add(currentUserReport1);
        timelineItems.Add(currentUserReport2);
        timelineItems.Add(otherUserReport1);
        timelineItems.Add(otherUserReport2);

        // 2. Create transport requests for animal1
        var transportRequest = new TransportRequest(
            DateTime.Now,
            otherUser.Id,
            animal1.Id,
            new MapPoint(10, 20),
            new MapPoint(30, 40),
            "Transport request for animal 1"
        );

        unitOfWork.TimelineItems.Add(transportRequest);
        timelineItems.Add(transportRequest);

        // 3. Create bills with contribution requests for animal2
        var bill = new Bill(
            DateTime.Now,
            otherUser.Id,
            animal2.Id,
            "Bill for animal 2",
            150000,
            false,
            false,
            false,
            null,
            RandomTestAttachment()
        );

        // Create contribution request and add a contribution from current user
        bill.RequestContribution();
        bill.Contribute(
            new Contribution(
                DateTime.Now,
                50000,
                currentUser.Id,
                "TRANSACTION_123",
                "Contribution from current user"
            ),
            true,
            false,
            true
        );

        unitOfWork.TimelineItems.Add(bill);
        timelineItems.Add(bill);

        // 4. Create comments
        var comments = new List<Comment>
        {
            new Comment(
                DateTime.Now,
                currentUser.Id,
                "Comment from current user on other user's report",
                otherUserReport1.Id
            ),
            new Comment(
                DateTime.Now,
                otherUser.Id,
                "Comment from other user on current user's report",
                currentUserReport1.Id
            )
        };

        foreach (var comment in comments)
        {
            unitOfWork.Comments.Add(comment);
        }

        // Save all changes
        await unitOfWork.Complete();

        return (currentUser, otherUser, animal1, animal2, timelineItems, comments);
    }
}