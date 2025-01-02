using System;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineItems;

public abstract class TimelineItem : AggregateRoot<TimelineItem>
{
    protected TimelineItem(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description)
    {
        EventDate = eventDate;
        Description = description;
        CreatedBy = createdBy;
        AnimalId = animalId;
    }

    protected TimelineItem(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId,
        string description) : this(eventDate, createdBy, animalId, description)
    {
        Id = id;
    }

    protected TimelineItem()
    {
    }

    public DateTime EventDate { get; private set; }
    public Id<Samaritan> CreatedBy { get; private set; }
    public Id<Animal> AnimalId { get; private set; }
    public string Description { get; private set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}