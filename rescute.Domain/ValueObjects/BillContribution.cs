using rescute.Domain.Aggregates;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class BillContribution: ValueObject
    {
        public DateTime Date { get; private set; }
        public long Amount { get; private set; }
        public string TransactionId { get; private set; }
        public Id<Samaritan> ContributorId { get; private set; }
        public string Description { get; private set; }

        public BillContribution(DateTime date, long amount, Id<Samaritan> contributorId, string transactionId,string description)
        {
            Date = date;
            Amount = amount;
            TransactionId = transactionId;
            ContributorId = contributorId;
            Description = description;
        }
    }
}
