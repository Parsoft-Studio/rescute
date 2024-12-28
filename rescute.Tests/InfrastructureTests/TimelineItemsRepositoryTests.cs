using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class TimelineItemsRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task TimelineItemsRepositoryAddsAndGetsEvent()
    {
        await using var unitOfWork = GetUnitOfWork();
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        animal.UpdateBirthCertificateId("birth_cert_id");

        var tEvent = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20),
            "This is the cat's status.",
            new Attachment("filename.jpg", "jpg", DateTime.Now, "Picture of the cat"));

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
        var contribution = new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID",
            contributionComment);

        var bill = new Bill(DateTime.Now,
            samaritan.Id,
            animal.Id,
            "I can't pay this on my own!",
            billAmount,
            includesLabResults,
            includesPrescription,
            includesVetFee,
            null,
            new Attachment("filename", "pdf", DateTime.Now, string.Empty)
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
}