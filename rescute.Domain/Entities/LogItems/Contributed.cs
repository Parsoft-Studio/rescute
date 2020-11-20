using rescute.Domain.Entities.LogItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class Contributed : ReportLogItem
    {
        public BillAttached Bill { get; private set; }
        public long Amount { get; private set; }
        public Contributed(DateTime eventDate, Aggregates.Samaritan createdBy, string description, BillAttached bill, long amount) : base(eventDate, createdBy, description)
        {
            Bill = bill;
            Amount = amount;
        }
        private Contributed() { }
    }
}
