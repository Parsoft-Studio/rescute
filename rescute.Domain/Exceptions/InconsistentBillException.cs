using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    public class InconsistentBillException:InvalidContributionException
    {
        public InconsistentBillException():base("The contributing Samaritan doesn't confirm this Bill's consistency: " +
                                        "Bill's owner claimed they have attached MedicalDocuments the Bill is paying for, " +
                                        "but the contribution Samaritan doesn't concur that all claimed documents are attached.")
        { }
    }
}
