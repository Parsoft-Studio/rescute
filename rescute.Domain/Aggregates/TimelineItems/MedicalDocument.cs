using System;
using System.Collections.Generic;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineItems;

public class MedicalDocument : TimelineItemWithAttachments

{
    public MedicalDocument(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description,
        MedicalDocumentType type, params Attachment[] documents) : base(eventDate, createdBy, animalId, description,
        documents)
    {
        Type = type;
    }

    public MedicalDocumentType Type { get; private set; }

    public override IReadOnlyList<AttachmentType> AcceptableAttachmentTypes =>
        [AttachmentType.Image(), AttachmentType.Document()];
}