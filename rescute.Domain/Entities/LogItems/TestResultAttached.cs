using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class TestResultAttached : DocumentedLogItem

    {
        public override IReadOnlyList<DocumentType> AcceptableDocumentTypes => new DocumentType[] { DocumentType.TestResult };
        public TestResultAttached(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Document[] documents) : base(eventDate, createdBy, description, documents)
        {

        }

        //public override object Clone()
        //{
        //    return this.MemberwiseClone();
        //}
    }
}
