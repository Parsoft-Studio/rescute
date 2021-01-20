using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;


namespace rescute.Domain.Aggregates.TimelineEvents
{
    public class StatusReported : TimelineEventWithAttachments, ICoordinated
    {
        public MapPoint EventLocation { get; private set; }

        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image, AttachmentType.Video };
        public StatusReported(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, MapPoint location, string description, params Attachment[] attachments) : base(eventDate, createdBy, animalId, description, attachments)
        {
            EventLocation = location;
        }
        private StatusReported() { }
    }
}
