using System;
using rescute.Domain.Aggregates;
using rescute.Shared;

namespace rescute.Domain.ValueObjects;

public record Contribution
{
    public DateTime Date { get; private set; }
    public decimal Amount { get;  private set;}
    public string TransactionId { get;  private set;}
    public Id<Samaritan> ContributorId { get;  private set;}
    public string Description { get;  private set;}

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