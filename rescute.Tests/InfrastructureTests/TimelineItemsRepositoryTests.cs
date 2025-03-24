using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Domain.ValueObjects;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class TimelineItemsRepositoryTests : RepositoryTestBase
{
    private static readonly DateTime DefaultDate = new(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc);

    [Fact]
    public async Task TimelineItemsRepositoryAddsAndGetsEvent()
    {
        await using var unitOfWork = GetUnitOfWork();
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        animal.UpdateBirthCertificateId("birth_cert_id");

        var tEvent = new StatusReport(DefaultDate, samaritan.Id, animal.Id, new MapPoint(10, 20),
            "This is the cat's status.",
            new Attachment("filename.jpg", "jpg", DefaultDate, "Picture of the cat"));

        unitOfWork.Animals.Add(animal);
        unitOfWork.Samaritans.Add(samaritan);
        unitOfWork.TimelineItems.Add(tEvent);


        await unitOfWork.Complete();
        var same = await unitOfWork.TimelineItems.GetAsync(tEvent.Id);

        same.Should().NotBe(null);
        same.Should().Be(tEvent);
        same.AnimalId.Should().Be(tEvent.AnimalId);
        same.CreatedBy.Should().Be(tEvent.CreatedBy);
    }

    [Fact]
    public async Task TimelineItemsRepositoryAddsAndGetsBillAndContribution()
    {
        await using var unitOfWork = GetUnitOfWork();
        var billAmount = 150000;
        var contributionComment = "Here, have this contribution.";
        var contributionAmount = 100000;
        bool includesVetFee = true, includesPrescription = false, includesLabResults = false;

        var samaritan = TestUtility.RandomTestSamaritan();
        var contributor = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var contribution = new Contribution(DefaultDate, contributionAmount, contributor.Id, "TRANSACTION_ID",
            contributionComment);

        var bill = new Bill(DefaultDate,
            samaritan.Id,
            animal.Id,
            "I can't pay this on my own!",
            billAmount,
            includesLabResults,
            includesPrescription,
            includesVetFee,
            null,
            new Attachment("filename", "pdf", DefaultDate, string.Empty)
        );

        bill.RequestContribution();
        bill.Contribute(contribution, includesLabResults, includesPrescription, includesVetFee);

        unitOfWork.Animals.Add(animal);
        unitOfWork.Samaritans.Add(samaritan);
        unitOfWork.Samaritans.Add(contributor);

        unitOfWork.TimelineItems.Add(bill);
        await unitOfWork.Complete();

        var stored = await unitOfWork.TimelineItems.GetAsync(bill.Id) as Bill;

        stored.Should().NotBeNull();
        stored.ContributionRequest.Contributions.Count.Should().Be(1);
        stored.ContributionRequest.Contributions.First().ContributorId.Should().Be(contributor.Id);
        stored.ContributionRequest.Contributions.First().Amount.Should().Be(contributionAmount);
        stored.CreatedBy.Should().Be(samaritan.Id);
    }
    [Fact]
    public async Task GetStatusReportsAsync_NoFilter_ReturnsAllStatusReports()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork(useDefaultDatabase: false);
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var currentUser = testData.CurrentUser;

        // Create filter with no restrictions
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            false, false, false, false, currentUser);

        // Act
        var result = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 10, 0);

        // Assert
        result.Should().HaveCount(4); // All status reports
        result.Should().AllBeOfType<StatusReport>();
    }

    [Fact]
    public async Task GetStatusReportsAsync_UsersReportsFilter_ReturnsOnlyCurrentUserReports()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork();
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var currentUser = testData.CurrentUser;

        // Create filter with UsersReports = true
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            true, false, false, false, currentUser);

        // Act
        var result = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 10, 0);

        // Assert
        result.Should().HaveCount(2); // Only current user's reports
        result.All(report => report.CreatedBy == currentUser.Id).Should()
            .BeTrue("all reports should be from current user");
    }

    [Fact]
    public async Task GetStatusReportsAsync_UsersContributionsFilter_ReturnsReportsUserContributedTo()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork();
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var currentUser = testData.CurrentUser;
        var animal2 = testData.Animal2;

        // Create filter with UsersContributions = true
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            false, true, false, false, currentUser);

        // Act
        var result = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 10, 0);

        // Assert
        // Should return reports for animal1 (comment contribution) and animal2 (bill contribution)
        result.Count.Should().BeGreaterThan(0);
        result.Should().Contain(report => report.AnimalId == animal2.Id);
    }

    [Fact]
    public async Task GetStatusReportsAsync_HasTransportRequestFilter_ReturnsReportsWithTransportRequests()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork();
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var currentUser = testData.CurrentUser;
        var animal1 = testData.Animal1;

        // Create filter with HasTransportRequest = true
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            false, false, true, false, currentUser);

        // Act
        var result = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 10, 0);

        // Assert
        result.Count.Should().BeGreaterThan(0);
        result.All(report => report.AnimalId == animal1.Id).Should().BeTrue("all reports should be for animal1");
    }

    [Fact]
    public async Task GetStatusReportsAsync_HasFundraiserRequestFilter_ReturnsReportsWithFundraiserRequests()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork();
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var currentUser = testData.CurrentUser;
        var animal2 = testData.Animal2;

        // Create filter with HasFundraiserRequest = true
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            false, false, false, true, currentUser);

        // Act
        var result = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 10, 0);

        // Assert
        result.Count.Should().BeGreaterThan(0);
        result.All(report => report.AnimalId == animal2.Id).Should().BeTrue("all reports should be for animal2");
    }

    [Fact]
    public async Task GetStatusReportsAsync_Pagination_RespectsPageSizeAndIndex()
    {
        // Arrange
        await using var unitOfWork = GetUnitOfWork();
        var testData = await TestUtility.CreateTimelineItemTestData(unitOfWork);
        var expectedIds = testData.TimelineItems.OfType<StatusReport>()
            .Select(r => r.Id).OrderBy(id => id).ToArray();
        
        var filter = ITimelineItemRepository.StatusReportFilter.CreateFilter(
            false, false, false, false, testData.CurrentUser);

        // Act - Get pages of size 2
        var page1 = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 2, 0);
        var page2 = await unitOfWork.TimelineItems.GetStatusReportsAsync(filter, 2, 1);

        // Assert
        page1.Should().HaveCount(2);
        page2.Should().HaveCount(2);
        page1.Select(r => r.Id).Should().NotIntersectWith(page2.Select(r => r.Id));
        
        var allIds = page1.Concat(page2)
            .Select(r => r.Id).OrderBy(id => id).ToArray();
        allIds.Should().BeEquivalentTo(expectedIds);
    }
    
}