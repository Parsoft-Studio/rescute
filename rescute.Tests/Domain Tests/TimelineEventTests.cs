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
        [Fact]
        public void BillRequestsContributions()
        {
            var billTotal = 5000;
            var samaritan = TestUtilities.RandomTestSamaritan();
            var attachment = new Attachment("test.pdf", "pdf", DateTime.Now, "Attachment description");

            var animal = TestUtilities.RandomTestAnimal(samaritan.Id);

            var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, "All the costs.", billTotal, false, false, true, null, attachment);

            var request = bill.RequestContribution();
            var internalRequest = bill.ContributionRequest;


            request.Should().NotBe(null);
            request.Should().Be(internalRequest);
        }
        [Fact]
        public void BillDoesntAcceptExcessContribution()
        {
            var transactionId = "TRANSACTION_ID";
            var billTotal = 5000;
            var samaritan = TestUtilities.RandomTestSamaritan();
            var contributor = TestUtilities.RandomTestSamaritan();
            var attachment = new Attachment("test.pdf", "pdf", DateTime.Now, "Attachment description");

            bool includesVetFee = true, includesPrescription = false, includesLabResults = false;

            var animal = TestUtilities.RandomTestAnimal(samaritan.Id);

            var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, "All the costs.", billTotal, includesLabResults, includesPrescription, includesVetFee, null, attachment);
            bill.RequestContribution();

            var contrib = new Contribution(DateTime.Now, billTotal + 1, contributor.Id, transactionId, "My contribution.");

            Action action = () =>
            {
                bill.Contribute(contrib, includesLabResults, includesPrescription, includesVetFee);
            };
            action.Should().Throw<ContributionExceedsRequirement>();
        }
        [Fact]
        public void BillIsConsistentWithAttachedMedicalDocuments()
        {
            var billAmount = 150000;
            var contribution_comment = "Here, have this contribution.";
            var contributionAmount = 100000;

            var samaritan = TestUtilities.RandomTestSamaritan();
            var animal = TestUtilities.RandomTestAnimal(samaritan.Id);
            var contributor = TestUtilities.RandomTestSamaritan();
            var contribution = new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contribution_comment);

            var billAttachment = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var labAttachment = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var prescriptionAttachment = new Attachment("filename", "jpg", DateTime.Now, string.Empty);

            var medicalDocuments = new List<MedicalDocument>();
            medicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.Prescription(), prescriptionAttachment));
            medicalDocuments.Add(new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, "Document description.", MedicalDocumentType.LabResults(), labAttachment));

            var bill = new Bill(DateTime.Now,
                samaritan.Id,
                animal.Id,
                "I can't pay this on my own!",
                billAmount,
                includesLabResults: true,
                includesPrescription: true,
                includesVetFee: true,
                medicalDocuments,
                billAttachment);


            bill.RequestContribution();
            Action action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: true,
                    includesPrescription: true,
                    includesVetFee: false);
            };
            action.Should().Throw<InconsistentBill>();
            action = () =>
             {
                 bill.Contribute(contribution,
                     includesLabResults: true,
                     includesPrescription: false,
                     includesVetFee: true);
             };
            action.Should().Throw<InconsistentBill>();
            action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: false,
                    includesPrescription: true,
                    includesVetFee: true);
            };
            action.Should().Throw<InconsistentBill>();

            action = () =>
            {
                bill.Contribute(contribution,
                    includesLabResults: true,
                    includesPrescription: true,
                    includesVetFee: true);
            };
            action.Should().NotThrow<InconsistentBill>();


        }
        [Fact]
        public void BillAcceptsContribution()
        {
            var billAmount = 150000;
            var contribution_comment = "Here, have this contribution.";
            var contributionAmount = 100000;

            var samaritan = TestUtilities.RandomTestSamaritan();
            var contributor = TestUtilities.RandomTestSamaritan();
            var contribution = new Contribution(DateTime.Now, contributionAmount, contributor.Id, "TRANSACTION_ID", contribution_comment);
            bool includesVetFee = true, includesPrescription = false, includesLabResults = false;

            var animal = TestUtilities.RandomTestAnimal(samaritan.Id);

            var bill = new Bill(DateTime.Now,
                samaritan.Id,
                animal.Id,
                "I can't pay this on my own!",
                billAmount,
                includesLabResults,
                includesPrescription,
                includesVetFee,
                null,
                new Attachment("filename", "pdf", DateTime.Now, string.Empty)
                );

            bill.RequestContribution();
            bill.Contribute(contribution, includesLabResults, includesPrescription, includesVetFee);


            var same = bill.ContributionRequest.Contributions.First();

            same.Should().NotBeNull();
            bill.ContributionRequest.Contributions.Count.Should().Be(1);
            same.ContributorId.Should().Be(contributor.Id);
            same.Amount.Should().Be(contributionAmount);
        }

        [Fact]
        public void EventsOnlyAcceptProperDocumentTypes()
        {
            var samaritan = TestUtilities.RandomTestSamaritan();
            var animal = TestUtilities.RandomTestAnimal(samaritan.Id);

            var docImage = new Attachment("filename", "jpg", DateTime.Now, string.Empty);
            var docVid = new Attachment("filename", "mp4", DateTime.Now, string.Empty);
            var docFile = new Attachment("filename", "fle", DateTime.Now, string.Empty);
            var docPdf = new Attachment("filename", "pdf", DateTime.Now, string.Empty);
            var point = new MapPoint(0, 0);

            // StatusReport
            var statusReportedValidAction = new Action(() =>
            {
                var statusReported = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid);
            });

            statusReportedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var statusReportedInvalidAction = new Action(() =>
            {
                var statusReported = new StatusReport(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid, docFile);
            });

            statusReportedInvalidAction.Should().Throw<InvalidAttachmentType>();


            // Bill
            var billAttachedValidAction = new Action(() =>
            {
                var bill = new Bill(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, false, false, false, null, docPdf, docImage);
            });

            billAttachedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var billAttachedInvalidAction = new Action(() =>
            {
                var billAttached = new Bill(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, false, false, false, null, docImage, docFile, docVid);
            });

            billAttachedInvalidAction.Should().Throw<InvalidAttachmentType>();


            // MedicalDocument
            var testResultAttachedValidAction = new Action(() =>
            {
                var videoAttached = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, string.Empty, MedicalDocumentType.DoctorsOrders(), docPdf);
            });

            testResultAttachedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var testResultAttachedInvalidAction = new Action(() =>
            {
                var videoAttached = new MedicalDocument(DateTime.Now, samaritan.Id, animal.Id, string.Empty, MedicalDocumentType.IdentityCertificate(), docVid, docFile);
            });

            testResultAttachedInvalidAction.Should().Throw<InvalidAttachmentType>();


        }



    }
}
