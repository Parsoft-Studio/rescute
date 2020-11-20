using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class Document : ValueObject
    {
        public string FileName { get; private set; }
        public DocumentType Type { get; private set; }
        public string Alias { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Document(DocumentType type, string filename, DateTime creationDate)
        {
            FileName = filename;
            Type = type;
            Alias = Guid.NewGuid().ToString();
            CreationDate = creationDate;
        }
        private Document() { }
    }
}
