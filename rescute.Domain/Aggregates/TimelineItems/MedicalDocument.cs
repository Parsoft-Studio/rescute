using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineItems
{
    public class MedicalDocument : TimelineItemWithAttachments

    {
        public MedicalDocumentType Type { get; private set; }
        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image(), AttachmentType.Document() };
        public MedicalDocument(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, MedicalDocumentType type, params Attachment[] documents) : base(eventDate, createdBy, animalId, description, documents)
        {
            Type = type;
        }
    }
}
