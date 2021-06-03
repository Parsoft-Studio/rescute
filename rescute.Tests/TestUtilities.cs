using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Tests
{
    class TestUtilities
    {
        public static Samaritan RandomTestSamaritan()
        {
            return new Samaritan(new Name("Test First"), new Name("Test Last"), new PhoneNumber(true, "0912121212"), DateTime.Now);
        }
        public static Animal RandomTestAnimal(Id<Samaritan> introducedBy)
        {
            return new Animal(DateTime.Now, introducedBy, "test animal", AnimalType.Cat());
        }

    }
}
