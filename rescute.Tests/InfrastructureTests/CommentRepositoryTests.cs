using System;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

// [Collection("Database collection")]
public class CommentRepositoryTests
{
    [Fact]
    public async Task CommentRepositoryAddsAndGetsComment()
    {
        using (var context = new rescuteContext(TestUtility.GetTestDatabaseOptions()))
        {
            using (var unitOfWork = new UnitOfWork(context))
            {
                var samaritan = TestUtility.RandomTestSamaritan();
                var animal = TestUtility.RandomTestAnimal(samaritan.Id);

                animal.UpdateBirthCertificateId("birth_cert_id");
                var tEvent = new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20),
                    new MapPoint(15, 25), "My description");
                var comment = new Comment(DateTime.Now, samaritan.Id,
                    "This is a comment on the transport requested event.", tEvent.Id);

                unitOfWork.Animals.Add(animal);
                unitOfWork.Samaritans.Add(samaritan);
                unitOfWork.TimelineItems.Add(tEvent);
                unitOfWork.Comments.Add(comment);

                await unitOfWork.Complete();
                var same = await unitOfWork.Comments.GetAsync(comment.Id);


                same.Should().NotBe(null);
                same.Should().Be(comment);
            }
        }
    }
}