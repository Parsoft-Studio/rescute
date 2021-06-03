using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class ContributionExceedsRequirement : ContributionException
    {
        public ContributionExceedsRequirement() : base("Contribution exceeds bill amount.")
        { }
    }
}
