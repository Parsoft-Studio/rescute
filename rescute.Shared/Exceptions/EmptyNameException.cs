using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Shared.Exceptions
{
    public class NameEmptyException : InvalidNameException
    {
        public NameEmptyException() : base("Name cannot be empty.")
        {
        }        
    }
}
