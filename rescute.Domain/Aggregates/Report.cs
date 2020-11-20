using System;
using System.Collections.Generic;
using System.Text;
using rescute.Domain.ValueObjects;
using rescute.Domain.Entities.LogItems;
using rescute.Shared;
using rescute.Domain.Extensions;
using System.Linq;
using rescute.Domain.Exceptions;

namespace rescute.Domain.Aggregates
{
    public class Report : AggregateRoot<Report>
    {
        private readonly List<ReportLogItem> logs = new List<ReportLogItem>();
        private readonly List<Animal> animals = new List<Animal>();
        public IReadOnlyCollection<ReportLogItem> Logs => logs.DeepCopy();
        public IReadOnlyCollection<Animal> Animals => animals;
        public Samaritan Samaritan { get; private set; }
        public DateTime CreationDate { get; private set; }
        public int CaseNumber { get; private set; }
        private Report() { }
        private Report(DateTime creationDate, Samaritan createdBy, params Animal[] animalsParams)
        {
            Samaritan = createdBy;
            CreationDate = creationDate;
            animals.AddRange(animalsParams);
        }

        public static Report New(DateTime creationDate, Samaritan createdBy, MapPoint location, string description, params Animal[] animalsParams)
        {
            Report report = new Report(creationDate, createdBy, animalsParams);
            report.logs.Add(new ReportCreated(creationDate, createdBy, location, description));

            return report;
        }

        public TransportRequested RequestTransport(Samaritan samaritan, MapPoint fromLocation, MapPoint toLocation, string comments)
        {
            var transportEvent = new TransportRequested(DateTime.Now, samaritan, fromLocation, toLocation, comments);
            this.logs.Add(transportEvent);
            return transportEvent.Clone() as TransportRequested;
        }

        public BillAttached AttachBill(Samaritan attachedBy, Document bill, long billTotal, bool requestingContributions, string comments)
        {
            var billAttachedEvent = new BillAttached(DateTime.Now, attachedBy, comments, bill);
            billAttachedEvent.UpdateContributionReqested(requestingContributions);
            billAttachedEvent.UpdateTotal(billTotal);
            this.logs.Add(billAttachedEvent);
            return billAttachedEvent.Clone() as BillAttached;
        }

        public Contributed Contribute(BillAttached bill, Samaritan contributor, long amount, string comments)
        {
            if (!logs.Contains(bill)) { throw new InvalidOperationException(); }

            AmountExceedsRequirement(bill, amount);

            var contribution = new Contributed(DateTime.Now, contributor, comments, bill, amount);
            this.logs.Add(contribution);
            return contribution.Clone() as Contributed;
        }
        public Commented Comment(Samaritan commenter, string comment, ReportLogItem replyTo)
        {
            if (!logs.Contains(replyTo)) { throw new InvalidOperationException(); }

            var commented = new Commented(DateTime.Now, commenter, comment, replyTo);
            this.logs.Add(commented);
            return commented.Clone() as Commented;
        }

        private void AmountExceedsRequirement(BillAttached bill, long amount)
        {
            var contriLogs = logs.Where(l => l.GetType() == typeof(Contributed) && ((Contributed)l).Bill == bill);
            var contributions = contriLogs.Select(l => ((Contributed)l).Amount);

            if (contributions.Count() > 0)
            {
                var total = contributions.Aggregate((total, next) => total += next);
                if ((total + amount) > bill.Total) { throw new ContributionExceedsRequirement(); }
            }
        }

    }
}
