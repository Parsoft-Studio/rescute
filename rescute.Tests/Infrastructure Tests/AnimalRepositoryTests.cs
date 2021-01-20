﻿using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class AnimalRepositoryTests
    {

        [Fact]
        public async void AnimalRepositoryAddsAndGetsAnimal()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Ehsan"), new Name("Hosseinkhani"), new PhoneNumber(true, "09355242601"), DateTime.Now);
                    var animal = new Animal(DateTime.Now, samaritan.Id, "This is my good pet.", AnimalType.Cat);
                    animal.UpdateBirthCertificateId("birth_cert_id");

                    unitOfWork.Animals.Add(animal);
                    unitOfWork.Samaritans.Add(samaritan);

                    await unitOfWork.Complete();
                    var same = await unitOfWork.Animals.GetAsync(animal.Id);


                    same.Should().NotBe(null);
                    same.Should().Be(animal);
                }
            }
        }
    }
}
