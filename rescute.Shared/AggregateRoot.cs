namespace rescute.Shared;

public abstract class AggregateRoot<T> : Entity<T> where T : Entity<T>
{
}