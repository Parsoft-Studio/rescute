using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace rescute.Domain.Entities.LogItems
{
    public class StatusReported : LogItemWithAttachments, ICoordinated
    {
        public MapPoint EventLocation { get; private set; }

        public override IReadOnlyList<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image, AttachmentType.Video };
        public StatusReported(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, MapPoint location, string description, params Attachment[] documents) : base(eventDate, createdBy, description, documents)
        {
            EventLocation = location;
        }
        private StatusReported() { }
    }
}
