using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates.TimelineItems;

/// <summary>
///     Represents an event with a location and a destination.
/// </summary>
public interface ITransport : ICoordinated
{
    public MapPoint ToLocation { get; }
}