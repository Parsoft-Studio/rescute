using System;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class CommentRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task CommentRepositoryAddsAndGetsComment()
    {
        await using var unitOfWork = GetUnitOfWork();
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);

        animal.UpdateBirthCertificateId("birth_cert_id");
        var transportRequest = new TransportRequest(DateTime.Now, samaritan.Id, animal.Id, new MapPoint(10, 20),
            new MapPoint(15, 25), "My description");
        var comment = new Comment(DateTime.Now, samaritan.Id,
            "This is a comment on the transport requested event.", transportRequest.Id);

        unitOfWork.Animals.Add(animal);
        unitOfWork.Samaritans.Add(samaritan);
        unitOfWork.TimelineItems.Add(transportRequest);
        unitOfWork.Comments.Add(comment);

        await unitOfWork.Complete();
        
        await using var otherUnitOfWork = GetUnitOfWork();
        var stored = await otherUnitOfWork.Comments.GetAsync(comment.Id);
        
        stored.Should().NotBe(null);
        stored.Should().Be(comment);
    }
}