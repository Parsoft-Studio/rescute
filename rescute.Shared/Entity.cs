using System;

namespace rescute.Shared
{
    public abstract class Entity<T> where T : Entity<T>
    {
        public Id<T> Id { get; protected set; }

        public Entity()
        {
            Id = Id<T>.Generate();
        }
        public override bool Equals(object obj)
        {
            var other = obj as Entity<T>;
            if (obj is null) return false;
            return ReferenceEquals(this, other) || this.Id == other.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> first, Entity<T> second)
        {
            if (first is null && second is null) return true;
            if (first is null || second is null) return false;
            return first.Equals(second);
        }
        public static bool operator !=(Entity<T> first, Entity<T> second)
        {
            return !(first == second);
        }

    }
}
