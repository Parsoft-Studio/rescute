using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Shared;

namespace rescute.Domain.Aggregates;

/// <summary>
///     Represents an item that can be considered as a reply to a <see cref="TimelineItem" />.
/// </summary>
public interface ITimelineItemReplier
{
    Id<TimelineItem> RepliesTo { get; }
}