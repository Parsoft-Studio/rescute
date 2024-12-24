using System;
using rescute.Domain.Aggregates;
using rescute.Shared;

namespace rescute.Domain.ValueObjects;

public record Contribution
{
    public DateTime Date { get; }
    public decimal Amount { get; }
    public string TransactionId { get; }
    public Id<Samaritan> ContributorId { get; }
    public string Description { get; }

    public Contribution(DateTime date, decimal amount, Id<Samaritan> contributorId, string transactionId,
        string description)
    {
        Date = date;
        Amount = amount;
        TransactionId = transactionId;
        ContributorId = contributorId;
        Description = description;
    }

    private Contribution()
    {
    }
}