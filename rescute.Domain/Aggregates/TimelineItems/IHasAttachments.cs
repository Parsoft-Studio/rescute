using System.Collections.Generic;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineItems;

/// <summary>
///     Represents an item that has a list of attachments.
/// </summary>
public interface IHasAttachments
{
    IReadOnlyList<Attachment> Attachments { get; }
    IReadOnlyList<AttachmentType> AcceptableAttachmentTypes { get; }
    void AddAttachments(params Attachment[] attachments);
    void ClearAttachments();
    void RemoveAttachment(Attachment attachment);
}