using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class Commented : LogItem, ILogItemReplier
    {
        public Commented(DateTime eventDate, Aggregates.Samaritan createdBy, string comment, LogItem replyTo) : base(eventDate, createdBy, comment)
        {
            RepliesTo = replyTo;
        }

        public LogItem RepliesTo { get; private set; }

        private Commented() { }
    }
}
