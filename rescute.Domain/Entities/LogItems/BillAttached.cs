using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public class BillAttached : DocumentedLogItem

    {
        public long Total { get; private set; }
        public bool ContributionRequested { get; private set; }

        public override IReadOnlyList<DocumentType> AcceptableDocumentTypes => new DocumentType[] { DocumentType.Bill };

        public BillAttached(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Document[] documents) : base(eventDate, createdBy, description, documents) { }

        public void UpdateTotal(long newTotal)
        {
            this.Total = newTotal;
        }
        public void UpdateContributionReqested(bool requested)
        {
            ContributionRequested = requested;
        }
        private BillAttached() { }
    }
}
