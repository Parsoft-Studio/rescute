using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineItems
{
    public abstract class TimelineItem : AggregateRoot<TimelineItem>
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public DateTime EventDate { get; private set; }
        public Id<Samaritan> CreatedBy { get; private set; }
        public Id<Animal> AnimalId { get; private set; }
        public string Description { get; private set; }
        public TimelineItem(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description)
        {
            EventDate = eventDate;
            Description = description;
            CreatedBy = createdBy;
            AnimalId = animalId;
        }
        public TimelineItem(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description) : this(eventDate, createdBy, animalId, description)
        {
            Id = id;
        }

        protected TimelineItem() { }
    }
}
