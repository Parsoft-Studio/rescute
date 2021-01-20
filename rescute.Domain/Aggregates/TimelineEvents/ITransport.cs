using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    /// <summary>
    /// Represents an event with a location and a destination.
    /// </summary>
    interface ITransport : ICoordinated
    {
        public MapPoint ToLocation { get; }
    }
}
