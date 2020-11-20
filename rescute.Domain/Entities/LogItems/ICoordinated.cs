using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    /// <summary>
    /// Represents an event with a geographical location on the map.
    /// </summary>
    interface ICoordinated
    {
        MapPoint EventLocation { get; }
    }
}
