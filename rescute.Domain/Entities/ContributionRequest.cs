using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Aggregates.TimelineItems
{
    public class ContributionRequest : Entity<ContributionRequest>
    {
        public DateTime RequestDate { get; private set; }
        public DateTime? RequestCompletionDate { get; private set; }
        
        public decimal ContributionsTotal => contributions.Count == 0 ? 0 : contributions.Select(c => c.Amount).Aggregate((total, next) => total += next);
        private readonly List<Contribution> contributions = new List<Contribution>();
        
        public IReadOnlyCollection<Contribution> Contributions => contributions.AsReadOnly();

        public Id<TimelineItem> BillId { get; private set; }

        public ContributionRequest(Id<TimelineItem> billId, DateTime requestDate) 
        {
            BillId = billId;
            this.RequestDate = requestDate;
        }
        private ContributionRequest() { }

        public void Contribute(Contribution contribution)
        {
            if (contribution == null) throw new ArgumentException("Contribution cannot be null.", nameof(contribution));
            contributions.Add(contribution);
        }
        public void SetRequestComplete(DateTime completionDate)
        {
            this.RequestCompletionDate = completionDate;
        }
    }
}
