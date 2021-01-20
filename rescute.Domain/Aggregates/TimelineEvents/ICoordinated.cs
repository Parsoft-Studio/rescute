using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    /// <summary>
    /// Represents an event with a geographical location on the map.
    /// </summary>
    interface ICoordinated
    {
        MapPoint EventLocation { get; }
    }
}
