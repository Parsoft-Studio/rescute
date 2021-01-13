using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Exceptions
{
    class MobilePhoneExpected : Exception
    {
        public MobilePhoneExpected() : base("Only mobile phone numbers are acceptable.")
        {

        }
    }
}
