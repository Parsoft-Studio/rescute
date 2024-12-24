using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Infrastructure;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

// [Collection("Database collection")]
public class AnimalRepositoryTests
{
    [Fact]
    public async Task AnimalRepositoryAddsAndGetsAnimals()
    {
        // Arrange
        var animals = new List<Animal>();
        var samaritan = TestUtility.RandomTestSamaritan();

        using var firstContext = new rescuteContext(TestUtility.GetTestDatabaseOptions());
        using var firstUnitOfWork = new UnitOfWork(firstContext);

        for (var i = 1; i <= 10; i++)
        {
            Animal animal = TestUtility.RandomTestAnimal(samaritan.Id);
            animal.UpdateBirthCertificateId("birth_cert_id");
            firstUnitOfWork.Animals.Add(animal);
            animals.Add(animal);
        }

        firstUnitOfWork.Samaritans.Add(samaritan);
        // Act
        await firstUnitOfWork.Complete();

        // Assert
        using var secondContext = new rescuteContext(TestUtility.GetTestDatabaseOptions());
        using var secondUnitOfWork = new UnitOfWork(secondContext);

        var stored = await secondUnitOfWork.Animals.GetAsync(a => true);

        stored.Should().NotBeNull();
        animals.TrueForAll(a => stored.Contains(a)).Should().Be(true);
        stored.All(a => a.Type != null).Should().Be(true);
    }
}