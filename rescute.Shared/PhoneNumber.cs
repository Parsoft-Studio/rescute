using rescute.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared
{
    public class PhoneNumber : ValueObject
    {
        public static PhoneNumber Empty => new PhoneNumber();
        public bool IsMobile { get; private set; }
        public string Value { get; private set; }

        public PhoneNumber(bool isMobile, string value)
        {
            if (isMobile)
            {
                if (isMobile && !(value.StartsWith("09") || value.StartsWith("+989"))) { throw new InvalidPhoneNumberException(); }
                Value = value;
                IsMobile = isMobile;
            }
        }
        public override string ToString()
        {
            return Value;
        }
        private PhoneNumber()
        {

        }
    }
}
