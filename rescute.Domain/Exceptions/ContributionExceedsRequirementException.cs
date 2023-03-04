using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class ContributionExceedsRequirementException : InvalidContributionException
    {
        public ContributionExceedsRequirementException() : base("Contribution exceeds bill amount.")
        { }
    }
}
