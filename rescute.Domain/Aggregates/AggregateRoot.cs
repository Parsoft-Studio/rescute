namespace rescute.Domain.Aggregates;

public abstract class AggregateRoot<T> : Entity<T> where T : Entity<T>
{
}