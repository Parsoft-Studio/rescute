﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class MobilePhoneExpectedException : Exception
    {
        public MobilePhoneExpectedException() : base("Only mobile phone numbers are acceptable.")
        {

        }
    }
}
