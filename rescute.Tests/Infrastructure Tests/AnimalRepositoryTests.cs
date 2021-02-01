using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class AnimalRepositoryTests
    {

        [Fact]
        public async void AnimalRepositoryAddsAndGetsAnimals()
        {
            // Arrange
            var animals = new List<Animal>();
            var samaritan = new Samaritan(new Name("Ehsan"), new Name("Hosseinkhani"), new PhoneNumber(true, "09355242601"), DateTime.Now);

            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {


                    Animal animal;

                    for (int i = 1; i <= 10; i++)
                    {
                        animal = new Animal(DateTime.Now, samaritan.Id, "This is my good pet.", AnimalType.Cat());
                        animal.UpdateBirthCertificateId("birth_cert_id");
                        unitOfWork.Animals.Add(animal);
                        animals.Add(animal);
                    }

                    unitOfWork.Samaritans.Add(samaritan);
                    // Act
                    await unitOfWork.Complete();
                }

            }
            // Assert
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    {

                        var stored = await unitOfWork.Animals.GetAsync(a => true);

                        stored.Should().NotBeNull();
                        animals.All(a => stored.Contains(a)).Should().Be(true);
                        stored.All(a => a.Type != null).Should().Be(true);
                    }
                }

            }
        }
    }
}
