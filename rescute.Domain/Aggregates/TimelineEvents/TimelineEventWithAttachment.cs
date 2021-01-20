using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    /// <summary>
    /// Represents a <see cref="TimelineEvent" that can have <see cref="Attachment"/>s.
    /// </summary>
    public abstract class TimelineEventWithAttachments :  TimelineEvent, IHasAttachments
    {
        private readonly List<Attachment> attachments = new List<Attachment>();
        public IReadOnlyCollection<Attachment> Attachments => attachments.AsReadOnly();
        public abstract IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes { get; }


        public TimelineEventWithAttachments(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, params Attachment[] attachments) : base(eventDate, createdBy,animalId, description)
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
        protected TimelineEventWithAttachments() { }
    }
}
