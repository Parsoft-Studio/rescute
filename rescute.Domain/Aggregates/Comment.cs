using System;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates;

public class Comment : AggregateRoot<Comment>, ITimelineItemReplier
{
    public Comment(DateTime date, Id<Samaritan> createdBy, string commentText, Id<TimelineItem> repliesTo)
    {
        RepliesTo = repliesTo;
        CommentText = commentText;
        CreatedBy = createdBy;
        Date = date;
    }

    private Comment()
    {
    }

    public DateTime Date { get; private set; }
    public string CommentText { get; private set; }
    public Id<Samaritan> CreatedBy { get; private set; }
    public Id<TimelineItem> RepliesTo { get; }
}