using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class AnimalType : ValueObject
    {
        public static AnimalType Cat = new AnimalType("Cat");
        public static AnimalType Dog = new AnimalType("Dog");
        public static AnimalType Sparrow = new AnimalType("Sparrow");
        public static AnimalType Pigeon = new AnimalType("Pigeon");
        public static AnimalType Other = new AnimalType("Other");
        public string Name { get; private set; }
        private AnimalType(string name)
        {
            Name = name;
        }
        private AnimalType() { }
    }
}
