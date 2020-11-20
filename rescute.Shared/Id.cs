using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared
{
    public sealed class Id<T> : ValueObject where T : Entity<T>
    {
        private Id(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }

        public static Id<T> Generate()
        {
            return new Id<T>(Guid.NewGuid());
        }
        public static Id<T> Generate(Guid fromGuid)
        {
            return new Id<T>(fromGuid);
        }
        public override string ToString()
        {
            return Value.ToString();

        }
    }
}
