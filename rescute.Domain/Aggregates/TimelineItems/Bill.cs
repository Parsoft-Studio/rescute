using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rescute.Domain.Aggregates.TimelineItems
{
    public class Bill : TimelineItemWithAttachments
    {
        public interface IContributionRequest
        {
            public Bill Bill { get; }
            IReadOnlyCollection<Contribution> Contributions { get; }
            decimal ContributionsTotal { get; }
            DateTime? RequestCompletionDate { get; }
            DateTime RequestDate { get; }
        }

        private List<Id<TimelineItem>> medicalDocumentIds = new List<Id<TimelineItem>>();
        public decimal Total { get; private set; }
        public IReadOnlyCollection<Id<TimelineItem>> MedicalDocumentIds => medicalDocumentIds.AsReadOnly();
        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image(), AttachmentType.Document() };
        public bool IncludesLabResults { get; private set; }
        public bool IncludesPrescription { get; private set; }
        public bool IncludesVetFee { get; private set; }
        private ContribRequest contributionRequest;
        public IContributionRequest ContributionRequest => contributionRequest;

        private Bill() { }
        public Bill(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, decimal total, bool includesLabResults, bool includesPrescription, bool includesVetFee, IEnumerable<MedicalDocument> medicalDocuments, params Attachment[] documents) : base(eventDate, createdBy, animalId, description, documents)
        {
            Total = total;

            UpdateInclusions(includesLabResults, includesPrescription, includesVetFee);
            UpdateMedicalDocuments(medicalDocuments, includesLabResults, includesPrescription);
        }

        public Bill(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, long total, bool includesLabResults, bool includesPrescription, bool includesVetFee, IEnumerable<MedicalDocument> medicalDocuments, params Attachment[] documents)
            : base(id, eventDate, createdBy, animalId, description, documents)
        {
            Total = total;

            UpdateInclusions(includesLabResults, includesPrescription, includesVetFee);
            UpdateMedicalDocuments(medicalDocuments, includesLabResults, includesPrescription);
        }

        /// <summary>
        /// Checks the consistency of claims made by this <see cref="Bill"/>'s owner about whether the <see cref="Bill"/> contains the <see cref="MedicalDocument"/>s it claims to pay for.
        /// </summary>
        /// <param name="includesLabResults">Whether the <see cref="Bill"/> actually contains lab results fee, to be checked against <see cref="Bill"/> owner's claim.</param>
        /// <param name="includesPrescription">Whether the <see cref="Bill"/> actually contains a prescription fee, to be checked against <see cref="Bill"/> owner's claim.</param>
        /// <param name="includesVetFee">Whether the <see cref="Bill"/> actually contains vet fees, to be checked against <see cref="Bill"/> owner's claim.</param>
        /// <returns></returns>
        public bool CheckConsistency(bool includesLabResults, bool includesPrescription, bool includesVetFee)
        {
            return (includesLabResults == IncludesLabResults &&
                includesPrescription == IncludesPrescription &&
                includesVetFee == IncludesVetFee);
        }

        /// <summary>
        /// Sets the values stated by this <see cref="Bill"/>'s owner that indicate whether the <see cref="Bill"/> includes lab results or prescriptions, etc.
        /// These values can later be checked against a contributing <see cref="Samaritan"/>'s verification to make sure the owner of the bill already attached what this <see cref="Bill"/>
        /// is supposed to pay for.
        /// </summary>
        /// <param name="includesLabResults"></param>
        /// <param name="includesPrescription"></param>
        /// <param name="includesVetFee"></param>
        public void UpdateInclusions(bool includesLabResults, bool includesPrescription, bool includesVetFee)
        {
            IncludesLabResults = includesLabResults;
            IncludesPrescription = includesPrescription;
            IncludesVetFee = includesVetFee;
        }

        /// <summary>
        /// Marks this <see cref="Bill"/> as one that other <see cref="Samaritan"/>s can contribute to.
        /// </summary>
        /// <returns></returns>
        public IContributionRequest RequestContribution()
        {
            contributionRequest = new ContribRequest(this, DateTime.Now);
            return ContributionRequest;
        }

        public void Contribute(Contribution contrib, bool includesLabResults, bool includesPrescription, bool includesVetFee)
        {
            if (ContributionRequest == null) throw new BillAcceptsNoContributionException();
            IsContributionValid(contrib.Amount, Total);
            contributionRequest.Contribute(contrib, includesLabResults, includesPrescription, includesVetFee);
        }

        public void UpdateMedicalDocuments(IEnumerable<MedicalDocument> documents, bool shouldHaveLabResults, bool shouldHavePrescription)
        {
            if (shouldHavePrescription)
            {
                if (documents == null || !documents.Any(doc => doc.Type == MedicalDocumentType.Prescription())) throw new MissingMedicalDocumentTypeException(MedicalDocumentType.Prescription());
            }

            if (shouldHaveLabResults)
            {
                if (documents == null || !documents.Any(doc => doc.Type == MedicalDocumentType.LabResults())) throw new MissingMedicalDocumentTypeException(MedicalDocumentType.LabResults());
            }
            medicalDocumentIds = documents == null ? medicalDocumentIds : documents.Select(doc => doc.Id).ToList();
        }

        public void UpdateTotal(long newTotal)
        {
            this.Total = newTotal;
        }
        private void IsContributionValid(decimal amount, decimal billTotal)
        {
            if ((this.ContributionRequest.ContributionsTotal + amount) > billTotal) { throw new ContributionExceedsRequirementException(); }
        }

        public class ContribRequest : Entity<ContribRequest>, IContributionRequest
        {
            public DateTime RequestDate { get; private set; }
            public DateTime? RequestCompletionDate { get; private set; }

            public decimal ContributionsTotal => contributions.Count == 0 ? 0 : contributions.Select(c => c.Amount).Aggregate((total, next) => total += next);
            private readonly List<Contribution> contributions = new List<Contribution>();

            public IReadOnlyCollection<Contribution> Contributions => contributions.AsReadOnly();

            public Bill Bill { get; private set; }
            public Id<TimelineItem> BillId { get; private set; }
            public ContribRequest(Bill bill, DateTime requestDate)
            {
                if (bill == null) throw new ArgumentException("Bill cannot be null.", nameof(bill));
                Bill = bill;
                BillId = bill.Id;
                RequestDate = requestDate;
            }
            private ContribRequest() { }

            public void Contribute(Contribution contribution, bool includesLabResults, bool includesPrescription, bool includesVetFee)
            {
                if (!Bill.CheckConsistency(includesLabResults, includesPrescription, includesVetFee)) throw new InconsistentBillException();
                if (contribution == null || contribution.Amount <= 0) throw new InvalidContributionException();
                contributions.Add(contribution);
            }
            public void SetRequestComplete(DateTime completionDate)
            {
                this.RequestCompletionDate = completionDate;
            }
        }
    }

}
