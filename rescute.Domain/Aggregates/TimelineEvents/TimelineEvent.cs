using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    public abstract class TimelineEvent: AggregateRoot<TimelineEvent> 
    {
        public  object Clone()
        {
            return this.MemberwiseClone();
        }
        
        public DateTime EventDate { get; private set; }
        public Id<Samaritan> CreatedBy { get; private set; }
        public Id<Animal> AnimalId { get; private set; }
        public string Description { get; private set; }
        public TimelineEvent(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description)
        {
            EventDate = eventDate;
            Description = description;
            CreatedBy = createdBy;
            AnimalId = animalId;
        }

        protected TimelineEvent() { }
    }
}
