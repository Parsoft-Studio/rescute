using rescute.Domain.Entities.LogItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class SamaritanContributed : LogItem
    {
        public BillAttached Bill { get; private set; }
        public long Amount { get; private set; }
        public SamaritanContributed(DateTime eventDate, Aggregates.Samaritan createdBy, string description, BillAttached bill, long amount) : base(eventDate, createdBy, description)
        {
            Bill = bill;
            Amount = amount;
        }
        private SamaritanContributed() { }
    }
}
