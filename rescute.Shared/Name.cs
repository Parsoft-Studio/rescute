using rescute.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared
{
    public class Name : ValueObject
    {
        public static Name Empty => new Name(string.Empty);
        public string Value { get; private set; }
        public int MaxLength { get; private set; }
        public Name(string value)
        {
            Value = value;
        }
        public Name(string value, int maxLength)
        {
            if (value.Length > maxLength) { throw new NameTooLongException(); }
            Value = value;
            MaxLength = maxLength;
        }
        public Name(string value, int maxLength, bool canBeEmpty)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(value)) { throw new NameEmptyException(); }
            if (value.Length > maxLength) { throw new NameTooLongException(); }
            Value = value;
            MaxLength = maxLength;

        }
        public Name() { }
        public bool IsNameEmpty() { return string.IsNullOrWhiteSpace(Value); }

        public override string ToString()
        {
            return Value;
        }
    }
}
