using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain
{
    /// <summary>
    /// Represents an item that has a list of attachments.
    /// </summary>
    public interface IHasAttachments
    {
        IReadOnlyList<Attachment> Attachments { get; }
        void AddAttachments(params Attachment[] attachments);
        void ClearAttachments();
        void RemoveAttachment(Attachment attachment);
        abstract IReadOnlyList<AttachmentType> AcceptableAttachmentTypes { get; }
    }
}
