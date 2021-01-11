using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class TransportRequested: LogItem, ITransport
    {
        public TransportRequested(DateTime eventDate, Aggregates.Samaritan createdBy, MapPoint from, MapPoint to, string description) : base(eventDate, createdBy, description)
        {
            EventLocation = from;
            ToLocation = to;
        }

        public MapPoint ToLocation { get; private set; }

        public MapPoint EventLocation { get; private set; }


        private TransportRequested() { }
    }
}
