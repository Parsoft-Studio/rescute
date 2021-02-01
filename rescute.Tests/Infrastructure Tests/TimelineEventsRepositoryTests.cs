using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using System.Linq;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class TimelineEventsRepositoryTests
    {

        [Fact]
        public async void TimelineEventsRepositoryAddsAndGetsEvent()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Ehsan"), new Name("Hosseinkhani"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    var animal = new Animal(DateTime.Now, samaritan.Id, "This is my good pet.", AnimalType.Cat());
                    animal.UpdateBirthCertificateId("birth_cert_id");

                    var tEvent = new StatusReported(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), "This is the cat's status.", new Attachment(AttachmentType.Image(), "filename.jpg", DateTime.Now, "Picture of the cat"));

                    unitOfWork.Animals.Add(animal);
                    unitOfWork.Samaritans.Add(samaritan);
                    unitOfWork.TimelineEvents.Add(tEvent);


                    await unitOfWork.Complete();
                    var same = await unitOfWork.TimelineEvents.GetAsync(tEvent.Id);

                    same.Should().NotBe(null);
                    same.Should().Be(tEvent);
                    same.AnimalId.Should().Be(tEvent.AnimalId);
                    same.CreatedBy.Should().Be(tEvent.CreatedBy);
                }
            }
        }
        [Fact]
        public async void TimelineEventsRepositoryAddsAndGetsBillAndContribution()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var billAmount = 150000;
                    var contribution_comment = "Here, have this contribution.";
                    var contributionAmount = 100000;

                    var samaritan = new Samaritan(new Shared.Name("Ehsan"), new Shared.Name("Hosseinkhani"), new Shared.PhoneNumber(true, "09355242601"), DateTime.Now);
                    var contributor = new Samaritan(new Shared.Name("Pooya"), new Shared.Name("Bisadi"), new Shared.PhoneNumber(true, "09385242601"), DateTime.Now);

                    var animal = new Animal(
                        registrationDate: DateTime.Now,
                        introducedBy: samaritan.Id,
                        description: "this is an animal",
                        type: AnimalType.Dog());

                    var bill = new BillAttached(DateTime.Now,
                        samaritan.Id,
                        animal.Id,
                        "I can't pay this on my own!",
                        billAmount,
                        true,
                        new Attachment(AttachmentType.Bill(), "filename", DateTime.Now, string.Empty)
                        );

                    bill.Contribute(new BillContribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contribution_comment));

                    unitOfWork.Animals.Add(animal);
                    unitOfWork.Samaritans.Add(samaritan);
                    unitOfWork.Samaritans.Add(contributor);

                    unitOfWork.TimelineEvents.Add(bill);
                    await unitOfWork.Complete();

                    var same = await unitOfWork.TimelineEvents.GetAsync(bill.Id) as BillAttached;

                    same.Should().NotBeNull();
                    same.Contributions.Count.Should().Be(1);
                    same.Contributions.First().ContributorId.Should().Be(contributor.Id);
                    same.CreatedBy.Should().Be(samaritan.Id);
                }
            }
        }

    }
}
