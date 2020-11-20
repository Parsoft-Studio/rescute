using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public abstract class ReportLogItem: Entity<ReportLogItem>
    {
        public  object Clone()
        {
            return this.MemberwiseClone();
        }
        
        public DateTime EventDate { get; private set; }
        public rescute.Domain.Aggregates.Samaritan Samaritan { get; private set; }
        public string Description { get; private set; }
        public ReportLogItem(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description)
        {
            EventDate = eventDate;
            this.Description = description;
            this.Samaritan = createdBy;
        }

        protected ReportLogItem() { }
    }
}
