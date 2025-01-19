using rescute.Domain.ValueObjects;

namespace rescute.Domain;

public abstract class Entity<T> where T : Entity<T>
{
    protected Entity()
    {
        Id = Id<T>.Generate();
    }

    public Id<T> Id { get; protected init; }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<T> other) return false;
        return this == other || Id == other.Id;
    }

    public override int GetHashCode()
    {
        return  Id.GetHashCode();
    }
}