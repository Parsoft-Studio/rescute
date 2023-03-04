using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    public class InvalidContributionException : Exception
    {
        public InvalidContributionException(string message = "Invalid contribution. Conbtribution cannot be null or have an invalid amount.") : base(message)
        { }
    }
}
