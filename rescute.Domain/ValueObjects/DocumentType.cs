using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class DocumentType : ValueObject
    {
        public static DocumentType Image = new DocumentType("Image");
        public static DocumentType Video = new DocumentType("Video");
        public static DocumentType TestResult = new DocumentType("TestResult");
        public static DocumentType Bill = new DocumentType("Bill");
        public string Type { get; private set; }
        public DocumentType(string type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return this.Type;
        }
        private DocumentType() { }
    }
}
