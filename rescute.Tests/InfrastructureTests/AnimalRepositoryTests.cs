using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using rescute.Domain.Aggregates;
using Xunit;

namespace rescute.Tests.InfrastructureTests;

public class AnimalRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task AnimalRepositoryAddsAndGetsAnimals()
    {
        var animals = new List<Animal>();
        var samaritan = TestUtility.RandomTestSamaritan();

        await using (var unitOfWork = GetUnitOfWork())
        {
            for (var i = 1; i <= 10; i++)
            {
                var animal = TestUtility.RandomTestAnimal(samaritan.Id);
                animal.UpdateBirthCertificateId("birth_cert_id");
                unitOfWork.Animals.Add(animal);
                animals.Add(animal);
            }

            unitOfWork.Samaritans.Add(samaritan);
        }

        // Assert

        await using (var unitOfWork = GetUnitOfWork())
        {
            var stored = await unitOfWork.Animals.GetAsync(a => true);

            stored.Should().NotBeNull();
            animals.TrueForAll(a => stored.Contains(a)).Should().Be(true);
            stored.All(a => a.Type != null).Should().Be(true);
        }
    }
}