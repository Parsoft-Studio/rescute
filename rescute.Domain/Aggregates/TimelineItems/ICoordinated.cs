using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineItems
{
    /// <summary>
    /// Represents an event with a geographical location on the map.
    /// </summary>
    public interface ICoordinated
    {
        MapPoint EventLocation { get; }
    }
}
