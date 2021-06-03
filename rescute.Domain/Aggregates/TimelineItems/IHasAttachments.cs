using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineItems
{
    /// <summary>
    /// Represents an item that has a list of attachments.
    /// </summary>
    public interface IHasAttachments
    {
        IReadOnlyCollection<Attachment> Attachments { get; }
        void AddAttachments(params Attachment[] attachments);
        void ClearAttachments();
        void RemoveAttachment(Attachment attachment);
        abstract IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes { get; }
    }
}
