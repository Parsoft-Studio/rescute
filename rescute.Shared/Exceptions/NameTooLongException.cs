using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared.Exceptions
{
    public class NameTooLongException : InvalidNameException
    {
        public NameTooLongException() : base("Name too long.")
        {

        }
    }
}
