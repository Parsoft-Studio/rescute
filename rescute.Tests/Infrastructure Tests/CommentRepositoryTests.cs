using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class CommentRepositoryTests
    {

        [Fact]
        public async void CommentRepositoryAddsAndGetsComment()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Ehsan"), new Name("Hosseinkhani"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    var animal = new Animal(DateTime.Now, samaritan.Id, "This is my good pet.", AnimalType.Cat());
                    animal.UpdateBirthCertificateId("birth_cert_id");
                    var tEvent = new TransportRequested(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20), new MapPoint(15, 25), "My description");
                    var comment = new Comment(DateTime.Now, samaritan.Id, "This is a comment on the transport requested event.", tEvent.Id);

                    unitOfWork.Animals.Add(animal);
                    unitOfWork.Samaritans.Add(samaritan);
                    unitOfWork.TimelineEvents.Add(tEvent);
                    unitOfWork.Comments.Add(comment);

                    await unitOfWork.Complete();
                    var same = await unitOfWork.Comments.GetAsync(comment.Id);


                    same.Should().NotBe(null);
                    same.Should().Be(comment);
                }
            }
        }
    }
}
