﻿using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates
{
    public  class VetClinic : AggregateRoot<VetClinic>
    {
        public string Title { get; private set; }
        public MapPoint Location { get; private set; }
        public VetClinic(string title, MapPoint location)
        {
            Title = title;
            Location = location;
        }
    }
}
