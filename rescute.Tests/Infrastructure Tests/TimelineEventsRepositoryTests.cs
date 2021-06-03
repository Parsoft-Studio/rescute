using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using System.Linq;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class TimelineItemsRepositoryTests
    {

        [Fact]
        public async void TimelineItemsRepositoryAddsAndGetsEvent()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = TestUtilities.RandomTestSamaritan();
                    var animal = TestUtilities.RandomTestAnimal(samaritan.Id);
                    animal.UpdateBirthCertificateId("birth_cert_id");

                    var tEvent = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), "This is the cat's status.", new Attachment("filename.jpg", "jpg", DateTime.Now, "Picture of the cat"));

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
            }
        }
        [Fact]
        public async void TimelineItemsRepositoryAddsAndGetsBillAndContribution()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var billAmount = 150000;
                    var contribution_comment = "Here, have this contribution.";
                    var contributionAmount = 100000;

                    var samaritan = TestUtilities.RandomTestSamaritan();
                    var contributor = TestUtilities.RandomTestSamaritan();

                    var animal = TestUtilities.RandomTestAnimal(samaritan.Id);

                    var bill = new Bill(DateTime.Now,
                        samaritan.Id,
                        animal.Id,
                        "I can't pay this on my own!",
                        billAmount,
                        false,
                        false,
                        false,
                        null,
                        new Attachment("filename", "pdf", DateTime.Now, string.Empty)
                        );

                    bill.RequestContribution();
                    bill.Contribute(new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contribution_comment));

                    unitOfWork.Animals.Add(animal);
                    unitOfWork.Samaritans.Add(samaritan);
                    unitOfWork.Samaritans.Add(contributor);

                    unitOfWork.TimelineItems.Add(bill);
                    await unitOfWork.Complete();

                    var same = await unitOfWork.TimelineItems.GetAsync(bill.Id) as Bill;

                    same.Should().NotBeNull();
                    same.ContributionRequest.Contributions.Count.Should().Be(1);
                    same.ContributionRequest.Contributions.First().ContributorId.Should().Be(contributor.Id);
                    same.ContributionRequest.Contributions.First().Amount.Should().Be(contributionAmount);
                    same.CreatedBy.Should().Be(samaritan.Id);
                }
            }
        }

    }
}
