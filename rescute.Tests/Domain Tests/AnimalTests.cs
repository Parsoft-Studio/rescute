using System;
using Xunit;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using FluentAssertions;
using System.Linq;
using rescute.Domain.Exceptions;
using System.Collections.Generic;
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Tests.DomainTests
{
    public class AnimalTests
    {


        [Fact]

        public void TwoAnimalsAreNotTheSame()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal1 = TestUtility.RandomTestAnimal(samaritan.Id);
            var animal2 = TestUtility.RandomTestAnimal(samaritan.Id);

            animal1.Should().NotBe(animal2);
        }

    }
}
