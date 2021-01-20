using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class ContributionNotRequested:Exception
    {
        public ContributionNotRequested() : base("Bill requires not contributions.") { }
    }
}
