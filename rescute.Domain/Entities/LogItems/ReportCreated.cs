using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace rescute.Domain.Entities.LogItems
{
    public class ReportCreated : DocumentedLogItem, ICoordinated
    {
        public MapPoint EventLocation { get; private set; }

        public override IReadOnlyList<DocumentType> AcceptableDocumentTypes => new DocumentType[] { DocumentType.Image, DocumentType.Video, DocumentType.TestResult, DocumentType.Bill };
        public ReportCreated(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, MapPoint location, string description, params Document[] documents) : base(eventDate, createdBy, description, documents)
        {
            EventLocation = location;
        }
        private ReportCreated() { }
    }
}
