using rescute.Shared;

namespace rescute.Domain.Aggregates.TimelineItems
{
    /// <summary>
    /// Represents an item that can be considered as a reply to a <see cref="TimelineItem"/>.
    /// </summary>
    public interface ITimelineItemReplier
    {
        Id<TimelineItem> RepliesTo { get; }
    }
}
