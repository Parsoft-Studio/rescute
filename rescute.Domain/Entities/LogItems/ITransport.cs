using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    /// <summary>
    /// Represents an event with a location and a destination.
    /// </summary>
    interface ITransport : ICoordinated
    {
        public MapPoint ToLocation { get; }
    }
}
