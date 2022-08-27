using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    class BillAcceptsNoContributionException : InvalidContributionException
    {
        public BillAcceptsNoContributionException() : base("The bill doesn't accept contributions.")
        { }
    }
}
