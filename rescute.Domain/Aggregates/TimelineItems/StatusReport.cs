using System;
using System.Collections.Generic;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineItems;

public class StatusReport : TimelineItemWithAttachments, ICoordinated
{
    public StatusReport(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, MapPoint location,
        string description, params Attachment[] attachments) : base(eventDate, createdBy, animalId, description,
        attachments)
    {
        EventLocation = location;
    }

    private StatusReport()
    {
    }

    public override IReadOnlyList<AttachmentType> AcceptableAttachmentTypes =>
        [AttachmentType.Image(), AttachmentType.Video()];

    public MapPoint EventLocation { get; }
}