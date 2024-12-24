using System.ComponentModel;

namespace rescute.Domain.ValueObjects;

public record AnimalType
{
    public static AnimalType Cat() => new("Cat");

    public static AnimalType Dog() => new("Dog");

    public static AnimalType Sparrow() => new("Sparrow");

    public static AnimalType Pigeon() => new("Pigeon");

    public static AnimalType Other() => new("Other");

    public string Name { get; }

    public AnimalType(string name)
    {
        Name = name;
    }

    private AnimalType()
    {
    }

    public static AnimalType GetByName(string animalTypeName)
    {
        if (animalTypeName.Equals(Cat().Name)) return Cat();
        if (animalTypeName.Equals(Dog().Name)) return Dog();
        if (animalTypeName.Equals(Sparrow().Name)) return Sparrow();
        if (animalTypeName.Equals(Pigeon().Name)) return Pigeon();
        if (animalTypeName.Equals(Other().Name)) return Other();

        throw new InvalidEnumArgumentException(nameof(animalTypeName));
    }

    public override string ToString()
    {
        return Name;
    }
}