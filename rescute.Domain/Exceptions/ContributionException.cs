using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    public abstract class ContributionException : Exception
    {
        public ContributionException(String message) : base(message)
        { }
    }
}
