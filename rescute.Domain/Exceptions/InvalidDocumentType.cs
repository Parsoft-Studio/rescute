using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class InvalidDocumentType : Exception
    {
        private List<DocumentType> types = new List<DocumentType>();
        public InvalidDocumentType(params DocumentType[] acceptableTypes)
        {
            types.AddRange(acceptableTypes);
        }
        public override string Message
        {
            get
            {
                string typeString = string.Empty;
                foreach (var t in types)
                {
                    typeString += t.ToString() + ", ";
                }
                return "Invalid document type. Acceptable types: " + typeString;
            }
        }
    }
}
