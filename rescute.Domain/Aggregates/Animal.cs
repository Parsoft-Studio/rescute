using rescute.Domain.Entities;
using rescute.Domain.Entities.LogItems;
using rescute.Domain.Exceptions;
using rescute.Domain.Extensions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Aggregates
{
    public class Animal : AggregateRoot<Animal>, IHasAttachments
    {
        private readonly List<LogItem> log = new List<LogItem>();
        private readonly List<Attachment> attachments = new List<Attachment>();
        public IReadOnlyCollection<LogItem> Log => log.OrderByDescending(l => l.EventDate).ToList().DeepCopy();
        public IReadOnlyList<Attachment> Attachments=> attachments.OrderByDescending(a => a.CreationDate).ToList().AsReadOnly();
        public string CaseNumber { get; private set; }
        public AnimalType Type { get; private set; }


        public IReadOnlyList<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.ProfilePicture };
        public DateTime RegistrationDate { get ; private set ; }
        public string Description { get ; private set ; }
        public string BirthCertificateId { get; private set; }
        public Samaritan IntroducedBy { get; private set; }
        private Animal() { }

        public static Animal New(DateTime registrationDate, Samaritan introducedBy, string description, AnimalType type)
        {
            var animal = new Animal();
            animal.UpdateRegistrationDate(registrationDate);
            animal.UpdateDescription(description);
            animal.UpdateIntroductionSamaritan(introducedBy);
            animal.UpdateAnimalType(type);

            return animal;
        }

        public void UpdateAnimalType(AnimalType type)
        {
            Type = type;
        }

        public void UpdateIntroductionSamaritan(Samaritan introducedBy)
        {
            IntroducedBy = introducedBy;
        }

        public void AddAttachments(params Attachment[] attachments)
        {
            if (attachments != null)
            {
                if (!attachments.All(d => this.AcceptableAttachmentTypes.Contains(d.Type))) throw new InvalidAttachmentType(AcceptableAttachmentTypes.ToArray());

                this.attachments.AddRange(attachments);
            }
        }

        public void ClearAttachments()
        {
            this.attachments.Clear();
        }

        public void RemoveAttachment(Attachment document)
        {
            this.attachments.Remove(document);
        }

        public void UpdateRegistrationDate(DateTime newRegDate)
        {
            RegistrationDate = newRegDate;
        }
        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }
        public void UpdateBirthCertificateId(string newId)
        {
            BirthCertificateId = newId;
        }
        public StatusReported ReportStatus(MapPoint location, Samaritan samaritan, string description, params Attachment[] attachments)
        {
            var statusEvent = new StatusReported(DateTime.Now, samaritan, location, description, attachments);
            log.Add(statusEvent);
            return statusEvent.Clone() as StatusReported;
        }

        public TransportRequested RequestTransport(Samaritan samaritan, MapPoint fromLocation, MapPoint toLocation, string comments)
        {
            var transportEvent = new TransportRequested(DateTime.Now, samaritan, fromLocation, toLocation, comments);
            log.Add(transportEvent);
            return transportEvent.Clone() as TransportRequested;
        }

        public BillAttached AttachBill(Samaritan attachedBy, Attachment bill, long billTotal, bool requestingContributions, string comments)
        {
            var billAttachedEvent = new BillAttached(DateTime.Now, attachedBy, comments, bill);
            billAttachedEvent.UpdateContributionReqested(requestingContributions);
            billAttachedEvent.UpdateTotal(billTotal);
            log.Add(billAttachedEvent);
            return billAttachedEvent.Clone() as BillAttached;
        }

        public SamaritanContributed SamaritanContributes(BillAttached bill, Samaritan contributor, long amount, string comments)
        {
            if (!log.Contains(bill)) { throw new InvalidOperationException(); }

            AmountExceedsRequirement(bill, amount);

            var contribution = new SamaritanContributed(DateTime.Now, contributor, comments, bill, amount);
            log.Add(contribution);
            return contribution.Clone() as SamaritanContributed;
        }
        public Commented SamaritanComments(Samaritan commenter, string comment, LogItem replyTo)
        {
            if (!log.Contains(replyTo)) { throw new InvalidOperationException(); }

            var commented = new Commented(DateTime.Now, commenter, comment, replyTo);
            log.Add(commented);
            return commented.Clone() as Commented;
        }

        private void AmountExceedsRequirement(BillAttached bill, long amount)
        {
            var contriLogs = log.Where(l => l.GetType() == typeof(SamaritanContributed) && ((SamaritanContributed)l).Bill == bill);
            var contributions = contriLogs.Select(l => ((SamaritanContributed)l).Amount);

            if (contributions.Count() > 0)
            {
                var total = contributions.Aggregate((total, next) => total += next);
                if ((total + amount) > bill.Total) { throw new ContributionExceedsRequirement(); }
            }
        }

    }
}
