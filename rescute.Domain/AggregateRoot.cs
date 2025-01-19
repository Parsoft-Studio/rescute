namespace rescute.Domain;

public abstract class AggregateRoot<T> : Entity<T> where T : Entity<T>
{
}