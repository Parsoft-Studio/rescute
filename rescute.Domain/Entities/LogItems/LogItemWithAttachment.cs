using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    /// <summary>
    /// Represents a <see cref="LogItem" that can have <see cref="Attachment"/>s.
    /// </summary>
    public abstract class LogItemWithAttachments :  LogItem, IHasAttachments
    {
        private readonly List<Attachment> attachments = new List<Attachment>();
        public IReadOnlyList<Attachment> Attachments => attachments.AsReadOnly();
        public abstract IReadOnlyList<AttachmentType> AcceptableAttachmentTypes { get; }


        public LogItemWithAttachments(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Attachment[] attachments) : base(eventDate, createdBy, description)
        {
            AddAttachments(attachments);
        }

        public void AddAttachments(params Attachment[] attachments)
        {
            if (attachments != null)
            {
                if (!attachments.All(d => this.AcceptableAttachmentTypes.Contains(d.Type))) throw new InvalidAttachmentType(AcceptableAttachmentTypes.ToArray());

                this.attachments.AddRange(attachments);
            }
        }

        public void ClearAttachments()
        {
            this.attachments.Clear();
        }

        public void RemoveAttachment(Attachment document)
        {
            this.attachments.Remove(document);
        }
        protected LogItemWithAttachments() { }
    }
}
