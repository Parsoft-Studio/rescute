using System;

namespace rescute.Shared
{
    public abstract class Entity<T> where T : Entity<T>
    {
        public Id<T> Id { get; private set; }

        public Entity()
        {
            Id = Id<T>.Generate();
        }
        public override bool Equals(object obj)
        {
            var other = obj as Entity<T>;
            if (ReferenceEquals(obj ,null)) return false;            
            if (ReferenceEquals(this, other)) return true;
            return this.Id == other.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Id.GetHashCode();
        }

        public static bool operator ==(Entity<T> first, Entity<T> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;
            return first.Equals(second);
        }
        public static bool operator !=(Entity<T> first, Entity<T> second)
        {
            return !(first == second);
        }

    }
}
