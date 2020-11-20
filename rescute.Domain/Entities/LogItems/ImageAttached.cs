using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class ImageAttached : DocumentedLogItem

    {
        public override IReadOnlyList<DocumentType> AcceptableDocumentTypes => new DocumentType[] { DocumentType.Image };

        public ImageAttached(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Document[] documents) : base(eventDate, createdBy, description, documents)
        {

        }
        private ImageAttached() { }
    }
}
