using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class ImageAttached : LogItemWithAttachments

    {
        public override IReadOnlyList<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image };

        public ImageAttached(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Attachment[] documents) : base(eventDate, createdBy, description, documents)
        {

        }
        private ImageAttached() { }
    }
}
