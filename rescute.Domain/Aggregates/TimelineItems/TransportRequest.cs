using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Aggregates.TimelineItems
{
    public class TransportRequest : TimelineItem, ITransport
    {
        public TransportRequest(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, MapPoint from, MapPoint to, string description) : base(eventDate, createdBy, animalId, description)
        {
            EventLocation = from;
            ToLocation = to;
        }

        public MapPoint ToLocation { get; private set; }

        public MapPoint EventLocation { get; private set; }


        private TransportRequest() { }
    }
}
