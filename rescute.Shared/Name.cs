using rescute.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared
{
    public class MyName : ValueObject
    {
        public static MyName Empty => new MyName(string.Empty);
        public string Value { get; private set; }
        public int MaxLength { get; private set; }
        public MyName(string value)
        {
            Value = value;
        }
        public MyName(string value, int maxLength)
        {
            if (value.Length > maxLength) { throw new NameTooLongException(); }
            Value = value;
            MaxLength = maxLength;
        }
        public MyName(string value, int maxLength, bool canBeEmpty)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(value)) { throw new NameEmptyException(); }
            if (value.Length > maxLength) { throw new NameTooLongException(); }
            Value = value;
            MaxLength = maxLength;

        }
        public MyName() { }
        public bool IsNameEmpty() { return string.IsNullOrWhiteSpace(Value); }

        public override string ToString()
        {
            return Value;
        }
    }
}
