using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class AnimalType : ValueObject
    {
        public static AnimalType Cat() { return new AnimalType("Cat"); }
        public static AnimalType Dog() { return new AnimalType("Dog"); }
        public static AnimalType Sparrow() { return new AnimalType("Sparrow"); }
        public static AnimalType Pigeon() { return new AnimalType("Pigeon"); }
        public static AnimalType Other() { return new AnimalType("Other"); }
        public string Name { get; private set; }
        private AnimalType(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        private AnimalType() { }

        public static AnimalType GetByName(string animalTypeName)
        {
            if (animalTypeName.ToLower() == Cat().Name.ToLower()) return Cat();
            if (animalTypeName.ToLower() == Dog().Name.ToLower()) return Dog();
            if (animalTypeName.ToLower() == Sparrow().Name.ToLower()) return Sparrow();
            if (animalTypeName.ToLower() == Pigeon().Name.ToLower()) return Pigeon();
            if (animalTypeName.ToLower() == Other().Name.ToLower()) return Other();
            return null;
        }
    }
}
