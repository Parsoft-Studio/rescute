using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class VideoAttached : DocumentedLogItem, ILogItemReplier

    {
        public override IReadOnlyList<DocumentType> AcceptableDocumentTypes => new DocumentType[] { DocumentType.Video };

        public ReportLogItem RepliesTo { get; private set; }

        public VideoAttached(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Document[] documents) : base(eventDate, createdBy, description, documents)
        {

        }

        //public override object Clone()
        //{
        //    return this.MemberwiseClone();
        //}
    }
}
