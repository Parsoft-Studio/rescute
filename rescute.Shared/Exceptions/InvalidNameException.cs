using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared.Exceptions
{
    public abstract class  InvalidNameException : Exception
    {
        public InvalidNameException(string message):base(message)
        {
        
        }
    }
}
