using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    public class InconsistentBill:InvalidContribution
    {
        public InconsistentBill():base("The contributing Samaritan doesn't confirm this Bill's consistency: " +
                                        "Bill's owner claimed they have attached MedicalDocuments the Bill is paying for, " +
                                        "but the contribution Samaritan doesn't concur that all claimed documents are attached.")
        { }
    }
}
