using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace rescute.Shared
{
    public abstract class ValueObject
    {
        public override bool Equals(object obj)
        {
            var other = obj as ValueObject;
            if (obj is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.GetHashCode() == other.GetHashCode();

        }
        public override int GetHashCode()
        {

            var props = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            int result = 0;
            foreach (var prop in props)
            {
                result = (result * 397) ^ prop.GetValue(this).GetHashCode();
            }            
            return result;
        }
        public static bool operator ==(ValueObject first, ValueObject second)
        {
            if (first is null && second is null) return true;
            if (first is null || second is null) return false;
            return first.Equals(second);
        }
        public static bool operator !=(ValueObject first, ValueObject second)
        {
            return !(first == second);
        }

    }
}
