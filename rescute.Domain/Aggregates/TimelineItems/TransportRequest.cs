using System;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineItems;

public class TransportRequest : TimelineItem, ITransport
{
    public MapPoint ToLocation { get; }
    public MapPoint EventLocation { get; }

    public TransportRequest(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, MapPoint from,
        MapPoint to, string description) : base(eventDate, createdBy, animalId, description)
    {
        EventLocation = from;
        ToLocation = to;
    }
    
    private TransportRequest()
    {
    }
}