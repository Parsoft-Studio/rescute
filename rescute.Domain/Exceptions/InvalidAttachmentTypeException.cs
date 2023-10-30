using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Exceptions
{
    public class InvalidAttachmentTypeException : Exception
    {
        private List<AttachmentType> types = new List<AttachmentType>();
        public InvalidAttachmentTypeException(params AttachmentType[] acceptableTypes)
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
                typeString.TrimEnd(',', ' ');
                return "Invalid attachment type. Acceptable types: " + typeString;
            }
        }
    }
}
