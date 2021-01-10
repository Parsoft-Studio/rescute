using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public abstract class LogItem: Entity<LogItem>
    {
        public  object Clone()
        {
            return this.MemberwiseClone();
        }
        
        public DateTime EventDate { get; private set; }
        public rescute.Domain.Aggregates.Samaritan Samaritan { get; private set; }
        public string Description { get; private set; }
        public LogItem(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description)
        {
            EventDate = eventDate;
            this.Description = description;
            this.Samaritan = createdBy;
        }

        protected LogItem() { }
    }
}
