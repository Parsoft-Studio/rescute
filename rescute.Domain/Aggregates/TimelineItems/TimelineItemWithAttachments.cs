using System;
using System.Collections.Generic;
using System.Linq;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineItems;

/// <summary>
///     Represents a <see cref="TimelineItem"/> that can have <see cref="Attachment" />s.
/// </summary>
public abstract class TimelineItemWithAttachments : TimelineItem, IHasAttachments
{
    private readonly List<Attachment> attachments = new();

    protected TimelineItemWithAttachments(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId,
        string description, params Attachment[] attachments) : base(eventDate, createdBy, animalId, description)
    {
        AddAttachments(attachments);
    }

    protected TimelineItemWithAttachments(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy,
        Id<Animal> animalId, string description, params Attachment[] attachments) : base(id, eventDate, createdBy,
        animalId, description)
    {
        AddAttachments(attachments);
    }

    protected TimelineItemWithAttachments()
    {
    }

    public IReadOnlyCollection<Attachment> Attachments => attachments.AsReadOnly();
    public abstract IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes { get; }

    public void AddAttachments(params Attachment[] attachments)
    {
        if (attachments == null || !attachments.Any())
            throw new ArgumentException("Attachments cannot be null or empty.", nameof(attachments));
        if (!Array.TrueForAll(attachments, d => AcceptableAttachmentTypes.Contains(d.Type)))
            throw new InvalidAttachmentTypeException(AcceptableAttachmentTypes.ToArray());
        this.attachments.AddRange(attachments);
    }

    public void ClearAttachments()
    {
        attachments.Clear();
    }

    public void RemoveAttachment(Attachment attachment)
    {
        attachments.Remove(attachment);
    }
}