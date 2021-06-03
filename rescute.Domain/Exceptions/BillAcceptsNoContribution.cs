﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    class BillAcceptsNoContribution : ContributionException
    {
        public BillAcceptsNoContribution() : base("The bill doesn't accept contributions.")
        { }
    }
}
