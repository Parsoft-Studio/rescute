using FluentAssertions;

using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace rescute.Tests.DomainTests
{
    public class TimelineItemTests
    {
        private readonly Samaritan _samaritan;
        private readonly Animal _animal;
        private readonly Attachment _pdfAttachment;
        private readonly Attachment _jpgAttachment;
        private readonly Attachment _mp4Attachment;
        private readonly Attachment _invalidAttachment;

        public TimelineItemTests()
        {
            _samaritan = TestUtility.RandomTestSamaritan();
            _animal = TestUtility.RandomTestAnimal(_samaritan.Id);
            _pdfAttachment = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            _jpgAttachment = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            _mp4Attachment = new Attachment("filename", "mp4", DateTime.Now, string.Empty);
            _invalidAttachment = new Attachment("filename", "fle", DateTime.Now, string.Empty);
        }

        [Fact]
        public void BillRequestsContributions()
        {
            var billTotal = 5000;

            var attachment = _pdfAttachment;
            var bill = new Bill(DateTime.Now, _samaritan.Id, _animal.Id, "All the costs.", billTotal, false, false, true, null, attachment);

            var request = bill.RequestContribution();
            var internalRequest = bill.ContributionRequest;

            request.Should().NotBe(null);
            request.Should().Be(internalRequest);
        }


        [Fact]
        public void BillDoesntAcceptExcessContribution()
        {
            const string transactionId = "TRANSACTION_ID";
            const int billTotal = 5000;

            var samaritan = TestUtility.RandomTestSamaritan();
            var contributor = TestUtility.RandomTestSamaritan();
            var attachment = new Attachment("test.pdf", "pdf", DateTime.Now, "Attachment description");

            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, "All the costs.", billTotal, false, false, true, null, attachment);
            bill.RequestContribution();

            var contrib = new Contribution(DateTime.Now, billTotal + 1, contributor.Id, transactionId, "My contribution.");

            Action action = () => bill.Contribute(contrib, false, false, true);

            action.Should().Throw<ContributionExceedsRequirementException>();
        }

        [Fact]
        public void BillIsConsistentWithAttachedMedicalDocuments()
        {
            var billAmount = 150000;
            var contribution_comment = "Here, have this contribution.";
            var contributionAmount = 100000;

            var contributor = TestUtility.RandomTestSamaritan();
            var contribution = new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contribution_comment);

            var medicalDocuments = new List<MedicalDocument>();
            medicalDocuments.Add(new MedicalDocument(DateTime.Now, _samaritan.Id, _animal.Id, "Document description.", MedicalDocumentType.Prescription(), _jpgAttachment));
            medicalDocuments.Add(new MedicalDocument(DateTime.Now, _samaritan.Id, _animal.Id, "Document description.", MedicalDocumentType.LabResults(), _jpgAttachment));

            var bill = new Bill(DateTime.Now,
                _samaritan.Id,
                _animal.Id,
                "I can't pay this on my own!",
                billAmount,
                includesLabResults: true,
                includesPrescription: true,
                includesVetFee: true,
                medicalDocuments,
                _pdfAttachment);

            bill.RequestContribution();

            Action action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: true,
                    includesPrescription: true,
                    includesVetFee: false);
            };
            action.Should().Throw<InconsistentBillException>();

            action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: true,
                    includesPrescription: false,
                    includesVetFee: true);
            };
            action.Should().Throw<InconsistentBillException>();

            action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: false,
                    includesPrescription: true,
                    includesVetFee: true);
            };
            action.Should().Throw<InconsistentBillException>();

            action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: true,
                    includesPrescription: true,
                    includesVetFee: true);
            };
            action.Should().NotThrow<InconsistentBillException>();
        }

        [Fact]
        public void ContributionIsAddedToBillWhenAccepted()
        {
            const int billAmount = 150000;
            const int contributionAmount = 100000;
            const string contributionComment = "Here, have this contribution.";
            var contributor = TestUtility.RandomTestSamaritan();
            var contribution = new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contributionComment);

            var bill = new Bill(DateTime.Now, _samaritan.Id, _animal.Id, "I can't pay this on my own!", billAmount, true, false, false, null, _pdfAttachment);
            bill.RequestContribution();
            bill.Contribute(contribution, true, false, false);

            bill.ContributionRequest.Contributions.Count.Should().Be(1);
            var same = bill.ContributionRequest.Contributions.First();
            same.ContributorId.Should().Be(contributor.Id);
            same.Amount.Should().Be(contributionAmount);
        }

        [Fact]
        public void StatusReportOnlyAcceptsProperDocumentTypes()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var docImage = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var docVid = new Attachment("filename", "mp4", DateTime.Now, string.Empty);
            var docFile = new Attachment("filename", "fle", DateTime.Now, string.Empty);
            var point = new MapPoint(0, 0);

            // StatusReport
            var statusReportedValidAction = new Action(() =>
            {
                var statusReported = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid);
            });

            statusReportedValidAction.Should().NotThrow<InvalidAttachmentTypeException>();

            var statusReportedInvalidAction = new Action(() =>
            {
                var statusReported = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid, docFile);
            });

            statusReportedInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
        }

        [Fact]
        public void BillOnlyAcceptsProperDocumentTypes()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var docImage = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var docVid = new Attachment("filename", "mp4", DateTime.Now, string.Empty);
            var docFile = new Attachment("filename", "fle", DateTime.Now, string.Empty);
            var docPdf = new Attachment("filename", "pdf", DateTime.Now, string.Empty);

            var billAttachedValidAction = new Action(() =>
            {
                var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, false, false, false, null, docPdf, docImage);
            });

            billAttachedValidAction.Should().NotThrow<InvalidAttachmentTypeException>();

            var billAttachedInvalidAction = new Action(() =>
            {
                var billAttached = new Bill(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, false, false, false, null, docImage, docFile, docVid);
            });

            billAttachedInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
        }

        [Fact]
        public void StatusReportDoesNotAcceptInvalidAttachments()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var docImage = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var docVid = new Attachment("filename", "mp4", DateTime.Now, string.Empty);
            var docFile = new Attachment("filename", "fle", DateTime.Now, string.Empty);
            var point = new MapPoint(0, 0);

            var validStatusReport = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid);

            var invalidStatusReport = new Action(() =>
            {
                var invalidDocImage = new Attachment("filename", "unknown", DateTime.Now, string.Empty);
                var invalidStatusReport = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, invalidDocImage, docVid);
            });

            invalidStatusReport.Should().Throw<InvalidAttachmentTypeException>();
        }


        [Fact]
        public void BillOnlyAcceptsValidAttachments()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var validAttachmentPdf = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var validAttachmentJpg = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var validAttachmentPng = new Attachment("filename", "png", DateTime.Now, string.Empty);

            var invalidAttachment = new Attachment("filename", "fle", DateTime.Now, string.Empty);

            var validMedicalDocuments = new List<MedicalDocument>();
            validMedicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.Prescription(), validAttachmentJpg));
            validMedicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.LabResults(), validAttachmentPng));

            Action action = () =>
            {
                var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, "Bill description.", 1000, false, false, false, validMedicalDocuments, validAttachmentPdf, invalidAttachment);
            };

            action.Should().Throw<InvalidAttachmentTypeException>();
        }

        [Fact]
        public void BillDoesNotAcceptInvalidAttachments()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);

            var validAttachmentPdf = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var validAttachmentJpg = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var validAttachmentPng = new Attachment("filename", "png", DateTime.Now, string.Empty);

            var invalidAttachment = new Attachment("filename", "fle", DateTime.Now, string.Empty);

            var validMedicalDocuments = new List<MedicalDocument>();
            validMedicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.Prescription(), validAttachmentJpg));
            validMedicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.LabResults(), validAttachmentPng));

            Action action = () =>
            {
                var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, "Bill description.", 1000, false, false, false, validMedicalDocuments, validAttachmentPdf, invalidAttachment);
            };

            action.Should().Throw<InvalidAttachmentTypeException>();
        }

        [Fact]
        public void MedicalDocumentOnlyAcceptsValidAttachments()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);
            var validPdfAttachment = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var validImageAttachment = new Attachment("filename", "jpg", DateTime.Now, string.Empty);

            var medicalDocumentValidAction = new Action(() =>
            {
                var medicalDoc = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description", MedicalDocumentType.Prescription(), validPdfAttachment, validImageAttachment);
            });

            medicalDocumentValidAction.Should().NotThrow<InvalidAttachmentTypeException>();
        }

        [Fact]
        public void MedicalDocumentDoesNotAcceptInvalidAttachments()
        {
            var samaritan = TestUtility.RandomTestSamaritan();
            var animal = TestUtility.RandomTestAnimal(samaritan.Id);
            var validPdfAttachment = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var invalidAttachment = new Attachment("filename", "txt", DateTime.Now, string.Empty);

            var medicalDocumentInvalidAction = new Action(() =>
            {
                var medicalDoc = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description", MedicalDocumentType.Prescription(), validPdfAttachment, invalidAttachment);
            });

            medicalDocumentInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
        }

    }
}
