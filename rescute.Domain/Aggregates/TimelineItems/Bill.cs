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
        private List<Id<TimelineItem>> medicalDocumentIds = new List<Id<TimelineItem>>();
        public decimal Total { get; private set; }
        public IReadOnlyCollection<Id<TimelineItem>> MedicalDocumentIds => medicalDocumentIds.AsReadOnly();
        public override IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.Image(), AttachmentType.Document() };
        public bool IncludesLabResults { get; private set; }
        public bool IncludesPrescription { get; private set; }
        public bool IncludesVetFee { get; private set; }
        public ContributionRequest ContributionRequest { get; private set; }

        private Bill() { }
        public Bill(DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, decimal total, bool includesLabResults, bool includesPrescription, bool includesVetFee, IEnumerable<MedicalDocument> medicalDocuments, params Attachment[] documents) : base(eventDate, createdBy, animalId, description, documents)
        {
            Total = total;

            IncludesLabResults = includesLabResults;
            IncludesPrescription = includesPrescription;
            IncludesVetFee = includesVetFee;

            SetMedicalDocuments(medicalDocuments, includesLabResults, includesPrescription);
        }

        public ContributionRequest RequestContribution()
        {
            ContributionRequest = new ContributionRequest(Id, DateTime.Now);
            return ContributionRequest;
        }

        public Bill(Id<TimelineItem> id, DateTime eventDate, Id<Samaritan> createdBy, Id<Animal> animalId, string description, long total, bool includesLabResults, bool includesPrescription, bool includesVetFee, IEnumerable<MedicalDocument> medicalDocuments, params Attachment[] documents)
            : base(id, eventDate, createdBy, animalId, description, documents)
        {
            Total = total;

            IncludesLabResults = includesLabResults;
            IncludesPrescription = includesPrescription;
            IncludesVetFee = includesVetFee;

            SetMedicalDocuments(medicalDocuments, includesLabResults, includesPrescription);
        }

        public void Contribute(Contribution contrib)
        {
            if (ContributionRequest == null) throw new BillAcceptsNoContribution();
            IsContributionValid(contrib.Amount, Total);
            ContributionRequest.Contribute(contrib);
        }

        private void SetMedicalDocuments(IEnumerable<MedicalDocument> documents, bool shouldHaveLabResults, bool shouldHavePrescription)
        {
            if (shouldHavePrescription)
            {
                if (documents == null ||  !documents.Any(doc => doc.Type == MedicalDocumentType.Prescription())) throw new MissingMedicalDocumentType(MedicalDocumentType.Prescription());
            }

            if (shouldHaveLabResults)
            {
                if (documents == null || !documents.Any(doc => doc.Type == MedicalDocumentType.LabResults())) throw new MissingMedicalDocumentType(MedicalDocumentType.LabResults());
            }
            medicalDocumentIds = documents == null ? medicalDocumentIds : documents.Select(doc => doc.Id).ToList();
        }

        public void UpdateTotal(long newTotal)
        {
            this.Total = newTotal;
        }
        private void IsContributionValid(decimal amount, decimal billTotal)
        {
            if ((this.ContributionRequest.ContributionsTotal + amount) > billTotal) { throw new ContributionExceedsRequirement(); }
        }

    }
}
