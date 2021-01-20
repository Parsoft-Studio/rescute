using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    /// <summary>
    /// Represents an item that can be considered as a reply to a <see cref="TimelineEvent"/>.
    /// </summary>
    public interface ITimelineEventReplier
    {
        Id<TimelineEvent> RepliesTo { get; }
    }
}
