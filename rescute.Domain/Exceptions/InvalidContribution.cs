using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    public class InvalidContribution : Exception
    {
        public InvalidContribution(string message = "Invalid contribution. Conbtribution cannot be null or have an invalid amount.") : base(message)
        { }
    }
}
