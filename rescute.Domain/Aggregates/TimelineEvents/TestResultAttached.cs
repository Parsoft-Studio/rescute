using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    public class TestResultAttached : TimelineEventWithAttachments

    {
        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.TestResult() };
        public TestResultAttached(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, params Attachment[] documents) : base(eventDate, createdBy, animalId, description, documents)
        {

        }
    }
}
