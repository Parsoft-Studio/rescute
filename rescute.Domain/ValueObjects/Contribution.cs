using rescute.Domain.Aggregates;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class Contribution : ValueObject
    {
        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }
        public string TransactionId { get; private set; }
        public Id<Samaritan> ContributorId { get; private set; }
        public string Description { get; private set; }

        public Contribution(DateTime date, decimal amount, Id<Samaritan> contributorId, string transactionId, string description)
        {
            Date = date;
            Amount = amount;
            TransactionId = transactionId;
            ContributorId = contributorId;
            Description = description;
        }
    }
}
