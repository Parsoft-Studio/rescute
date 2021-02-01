using System;
using Xunit;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using FluentAssertions;
using System.Linq;
using rescute.Domain.Exceptions;
using System.Collections.Generic;
using rescute.Domain.Aggregates.TimelineEvents;

namespace rescute.Tests.DomainTests
{
    public class AnimalTests
    {


        [Fact]

        public void TwoAnimalsAreNotTheSame()
        {
            var samaritan = new Samaritan(new Shared.Name("Ehsan"), new Shared.Name("Hosseinkhani"), new Shared.PhoneNumber(true, "09355242601"), DateTime.Now);
            var animal1 = new Animal(
                registrationDate: DateTime.Now,
                introducedBy: samaritan.Id,
                description: "this is an animal",
                type: AnimalType.Sparrow());

            var animal2 = new Animal(
                registrationDate: DateTime.Now,
                introducedBy: samaritan.Id,
                description: "this is an animal",
                type: AnimalType.Sparrow());

            animal1.Should().NotBe(animal2);
        }

    }
}
