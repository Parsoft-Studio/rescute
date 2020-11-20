using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class Commented : ReportLogItem, ILogItemReplier
    {
        public Commented(DateTime eventDate, Aggregates.Samaritan createdBy, string comment, ReportLogItem replyTo) : base(eventDate, createdBy, comment)
        {
            RepliesTo = replyTo;
        }

        public ReportLogItem RepliesTo { get; private set; }

        private Commented() { }
    }
}
