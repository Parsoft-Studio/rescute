using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using Xunit;

namespace rescute.Tests.DomainTests;

public class TimelineItemTests
{
    private static readonly DateTime DefaultDate = new(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc);

    private static readonly Attachment
        JpgAttachment = new("file.jpg", "jpg", DateTime.UtcNow, "description");

    private static readonly Attachment
        Mp4Attachment = new("file.mp4", "mp4", DateTime.UtcNow, "description");

    private static readonly Attachment
        PdfAttachment = new("file.pdf", "pdf", DateTime.UtcNow, "description");

    private static readonly Attachment
        MiscAttachment = new("filename", "fle", DateTime.UtcNow, "description");

    private static readonly MapPoint DefaultMapPoint = new(0, 0);

    private static readonly Samaritan DefaultSamaritan = TestUtility.RandomTestSamaritan();
    private static readonly Animal DefaultAnimal = TestUtility.RandomTestAnimal(DefaultSamaritan.Id);
    private const Decimal DefaultBillTotal = 5000;
    private const string DefaultTransactionId = "TRANSACTION_ID";
    private const string DefaultContributionDescription = "My contribution";

    [Fact]
    public void BillRequestsContributions()
    {
        var bill = new Bill(DefaultDate, DefaultSamaritan.Id, DefaultAnimal.Id, "All the costs.", DefaultBillTotal,
            false,
            false, true,
            null, PdfAttachment);

        var request = bill.RequestContribution();
        var internalRequest = bill.ContributionRequest;

        request.Should().NotBe(null);
        request.Should().Be(internalRequest);
    }


    [Fact]
    public void BillDoesNotAcceptExcessContribution()
    {
        var attachment = new Attachment("test.pdf", "pdf", DefaultDate, "Attachment description");

        var bill = new Bill(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            "All the costs.",
            DefaultBillTotal,
            false,
            false,
            true,
            null, attachment);
        bill.RequestContribution();

        var contribution = new Contribution(DefaultDate,
            DefaultBillTotal + 1, // more than the bill amount
            DefaultSamaritan.Id,
            DefaultTransactionId,
            DefaultContributionDescription);

        var action = () => bill.Contribute(contribution, false, false, true);

        action.Should().Throw<ContributionExceedsRequirementException>();
    }


    [Fact]
    public void BillIsConsistentWithAttachedMedicalDocuments()
    {
        const string documentDescription = "Document description";
        const string billDescription = "I can't pay this on my own!";
        var contributionAmount = DefaultBillTotal;

        var contribution = new Contribution(DefaultDate, contributionAmount, DefaultSamaritan.Id, DefaultTransactionId,
            DefaultContributionDescription);

        var medicalDocuments = new List<MedicalDocument>
        {
            new(DefaultDate, DefaultSamaritan.Id, DefaultAnimal.Id,
                documentDescription,
                MedicalDocumentType.Prescription(),
                JpgAttachment),
            new(DefaultDate, DefaultSamaritan.Id, DefaultAnimal.Id,
                documentDescription,
                MedicalDocumentType.LabResults(),
                JpgAttachment)
        };

        var bill = new Bill(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            billDescription,
            DefaultBillTotal,
            true,   // bill claims to pay for every kind of documents
            true,
            true,
            medicalDocuments,
            PdfAttachment);

        bill.RequestContribution();

        var action = () =>
        {
            bill.Contribute(contribution,
                true,
                true,
                false); //  contribution doesn't confirm bill's claim 
        };
        action.Should().Throw<InconsistentBillException>();

        action = () =>
        {
            bill.Contribute(contribution,
                true,
                false,  //  contribution doesn't confirm bill's claim
                true);
        };
        action.Should().Throw<InconsistentBillException>();

        action = () =>
        {
            bill.Contribute(contribution,
                false,  //  contribution doesn't confirm bill's claim
                true,
                true);
        };
        action.Should().Throw<InconsistentBillException>();

        action = () =>
        {
            bill.Contribute(contribution,   //  contribution confirms bill's claim
                true,
                true,
                true);
        };
        action.Should().NotThrow<InconsistentBillException>();
    }

    [Fact]
    public void ContributionIsAddedToBillWhenAccepted()
    {
        const decimal contributionAmount = DefaultBillTotal;

        var bill = new Bill(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            "I can't pay this on my own!",
            DefaultBillTotal,
            false,
            false,
            true,
            null,
            PdfAttachment);

        var contribution = new Contribution(DefaultDate,
            contributionAmount,
            DefaultSamaritan.Id,
            DefaultTransactionId,
            DefaultContributionDescription);

        bill.RequestContribution();

        bill.Contribute(contribution,
            false,
            false,
            true);

        bill.ContributionRequest
            .Contributions
            .Count.Should().Be(1);

        bill.ContributionRequest
            .Contributions
            .First()
            .Should().Be(contribution);
    }


    [Fact]
    public void StatusReportOnlyAcceptsValidAttachmentTypes()
    {
        var statusReportValidAction = new Func<StatusReport>(() => new StatusReport(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            DefaultMapPoint,
            string.Empty,
            JpgAttachment,
            Mp4Attachment));

        statusReportValidAction.Should().NotThrow<InvalidAttachmentTypeException>();

        var statusReportedInvalidAction = new Func<StatusReport>(() => new StatusReport(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            DefaultMapPoint,
            string.Empty,
            JpgAttachment,
            MiscAttachment, // invalid
            Mp4Attachment
        ));

        statusReportedInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
    }

    [Fact]
    public void BillOnlyAcceptsValidAttachmentTypes()
    {
        var billAttachedValidAction = new Func<Bill>(() => new Bill(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            string.Empty,
            DefaultBillTotal,
            false,
            false,
            false,
            null,
            PdfAttachment,
            JpgAttachment));

        billAttachedValidAction.Should().NotThrow<InvalidAttachmentTypeException>();

        var billAttachedInvalidAction = new Func<Bill>(() => new Bill(DefaultDate,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            string.Empty,
            DefaultBillTotal,
            false,
            false,
            false,
            null,
            JpgAttachment,
            MiscAttachment, // invalid
            Mp4Attachment));

        billAttachedInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
    }
    
    [Fact]
    public void MedicalDocumentOnlyAcceptsValidAttachmentTypes()
    {
        var medicalDocumentValidAction = new Func<MedicalDocument>(() => new MedicalDocument(DateTime.UtcNow,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            "Document description",
            MedicalDocumentType.Prescription(),
            PdfAttachment,
            JpgAttachment));

        medicalDocumentValidAction.Should().NotThrow<InvalidAttachmentTypeException>();

        var medicalDocumentInvalidAction = new Func<MedicalDocument>(() => new MedicalDocument(DateTime.UtcNow,
            DefaultSamaritan.Id,
            DefaultAnimal.Id,
            "Document description",
            MedicalDocumentType.Prescription(),
            PdfAttachment,
            MiscAttachment, // invalid
            JpgAttachment));

        medicalDocumentInvalidAction.Should().Throw<InvalidAttachmentTypeException>();
    }
}