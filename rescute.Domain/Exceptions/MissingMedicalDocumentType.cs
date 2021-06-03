using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Exceptions
{
    class MissingMedicalDocumentType : Exception
    {
        public MissingMedicalDocumentType(MedicalDocumentType missingType) : base($"The following medical document type expected but missing: {missingType}")
        {
            MissingType = missingType;
        }

        public MedicalDocumentType MissingType { get; }
    }
}
