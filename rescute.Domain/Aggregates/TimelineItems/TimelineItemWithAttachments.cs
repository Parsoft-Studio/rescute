using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineItems
{
    /// <summary>
    /// Represents a <see cref="TimelineItem" that can have <see cref="Attachment"/>s.
    /// </summary>
    public abstract class TimelineItemWithAttachments : TimelineItem, IHasAttachments
    {
        private readonly List<Attachment> attachments = new List<Attachment>();
        public IReadOnlyCollection<Attachment> Attachments => attachments.AsReadOnly();
        public abstract IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes { get; }

        public TimelineItemWithAttachments(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, params Attachment[] attachments) : base(eventDate, createdBy, animalId, description)
        {
            AddAttachments(attachments);
        }
        public TimelineItemWithAttachments(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, params Attachment[] attachments) : base(id, eventDate, createdBy, animalId, description)
        {
            AddAttachments(attachments);
        }

        public void AddAttachments(params Attachment[] attachments)
        {
            if (attachments == null || attachments.Count() == 0) throw new ArgumentException("Attachments cannot be null or empty.", nameof(attachments));
            if (!attachments.All(d => this.AcceptableAttachmentTypes.Contains(d.Type))) throw new InvalidAttachmentType(AcceptableAttachmentTypes.ToArray());
            this.attachments.AddRange(attachments);
        }

        public void ClearAttachments()
        {
            this.attachments.Clear();
        }

        public void RemoveAttachment(Attachment document)
        {
            this.attachments.Remove(document);
        }
        protected TimelineItemWithAttachments() { }
    }
}
