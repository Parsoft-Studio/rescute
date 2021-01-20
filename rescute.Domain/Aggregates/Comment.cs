using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Shared;
using System;

namespace rescute.Domain.Aggregates
{
    public class Comment : AggregateRoot<Comment>, ITimelineEventReplier
    {
        public Comment(DateTime date, Id<Samaritan> createdBy, string commentText, Id<TimelineEvent> repliesTo) 
        {
            RepliesTo = repliesTo;
            CommentText = commentText;
            CreatedBy = createdBy;
            Date = date;
        }

        public DateTime Date { get; private set; }
        public string CommentText { get; private set; }
        public Id<Samaritan> CreatedBy { get; private set; }
        public Id<TimelineEvent> RepliesTo { get; private set; }

        private Comment() { }
    }
}
