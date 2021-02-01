using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rescute.Domain.Aggregates.TimelineEvents
{
    public class BillAttached : TimelineEventWithAttachments

    {
        private readonly List<BillContribution> contributions = new List<BillContribution>();
        public long Total { get; private set; }
        public bool ContributionRequested { get; private set; }
        public long ContributionsTotal => contributions.Count == 0 ? 0: contributions.Select(c => c.Amount).Aggregate((total, next) => total += next);
        public IReadOnlyCollection<BillContribution> Contributions => contributions.AsReadOnly();
        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Bill() };

        public BillAttached(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, long total, bool contributionRequested, params Attachment[] documents) : base(eventDate, createdBy, animalId, description, documents)
        {
            Total = total;
            ContributionRequested = contributionRequested;
        }

        public void UpdateTotal(long newTotal)
        {
            IsContributionValid(0, newTotal);
            this.Total = newTotal;
        }
        public void UpdateContributionReqested(bool requested)
        {
            ContributionRequested = requested;
        }
        private BillAttached() { }
        private void IsContributionValid(long amount, long billTotal)
        {
            if ((ContributionsTotal + amount) > billTotal) { throw new ContributionExceedsRequirement(); }
        }
        public void Contribute(BillContribution contribution)
        {
            if (contribution == null) throw new ArgumentException("Contribution cannot be null.", nameof(contribution));
            if (!ContributionRequested) throw new ContributionNotRequested();
            IsContributionValid(contribution.Amount, Total);
            contributions.Add(contribution);
        }

    }
}
