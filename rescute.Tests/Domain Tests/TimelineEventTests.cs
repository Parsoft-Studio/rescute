using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
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
    public class TimelineEventTests
    {

        [Fact]
        public void BillDoesntAcceptExcessContribution()
        {
            var samaritan = new Samaritan(new Shared.Name("Ehsan"), new Shared.Name("Hosseinkhani"), new Shared.PhoneNumber(true, "09355242601"), DateTime.Now);
            var contributor = new Samaritan(new Shared.Name("Pooya"), new Shared.Name("Bisadi"), new Shared.PhoneNumber(true, "09385242601"), DateTime.Now);
            var billTotal = 5000;
            var animal = new Animal(
                DateTime.Now,
                introducedBy: samaritan.Id,
                description: "this is an animal",
                type: AnimalType.Pigeon());

            var bill = new BillAttached(
                DateTime.Now,
                samaritan.Id,
                animal.Id,
                 "I can't pay this on my own!",
                 billTotal,
                 true,
                new Attachment(AttachmentType.Bill(), "filename", DateTime.Now, string.Empty)
                );

            Action action = () =>
            {
                bill.Contribute(new BillContribution(DateTime.Now, 5001, contributor.Id, "TRANSACTION_ID", "Here."));
            };
            action.Should().Throw<ContributionExceedsRequirement>();
        }

        [Fact]
        public void EventsOnlyAcceptProperDocumentTypes()
        {
            var samaritan = new Samaritan(new Shared.Name("Ehsan"), new Shared.Name("Hosseinkhani"), new Shared.PhoneNumber(true, "09355242601"), DateTime.Now);
            var animal = new Animal(DateTime.Now, introducedBy: samaritan.Id, description: "this is an animal", type: AnimalType.Pigeon());
            var docImage = new Attachment(AttachmentType.Image(), "filename", DateTime.Now, string.Empty);
            var docVid = new Attachment(AttachmentType.Video(), "filename", DateTime.Now, string.Empty);
            var docTest = new Attachment(AttachmentType.TestResult(), "filename", DateTime.Now, string.Empty);
            var docBill = new Attachment(AttachmentType.Bill(), "filename", DateTime.Now, string.Empty);
            var point = new MapPoint(0, 0);

            // StatusReported
            var statusReportedValidAction = new Action(() =>
            {
                var statusReported = new StatusReported(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid);
            });

            statusReportedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var statusReportedInvalidAction = new Action(() =>
            {
                var statusReported = new StatusReported(DateTime.Now, samaritan.Id, animal.Id, point, string.Empty, docImage, docVid, docBill);
            });

            statusReportedInvalidAction.Should().Throw<InvalidAttachmentType>();


            // BillAttached
            var billAttachedValidAction = new Action(() =>
            {
                var billAttached = new BillAttached(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, true, docBill);
            });

            billAttachedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var billAttachedInvalidAction = new Action(() =>
            {
                var billAttached = new BillAttached(DateTime.Now, samaritan.Id, animal.Id, string.Empty, 1000, true, docImage, docTest, docVid);
            });

            billAttachedInvalidAction.Should().Throw<InvalidAttachmentType>();


            // TestResultAttached
            var testResultAttachedValidAction = new Action(() =>
            {
                var videoAttached = new TestResultAttached(DateTime.Now, samaritan.Id, animal.Id, string.Empty, docTest);
            });

            testResultAttachedValidAction.Should().NotThrow<InvalidAttachmentType>();

            var testResultAttachedInvalidAction = new Action(() =>
            {
                var videoAttached = new TestResultAttached(DateTime.Now, samaritan.Id,animal.Id, string.Empty, docImage, docTest, docVid, docBill);
            });

            testResultAttachedInvalidAction.Should().Throw<InvalidAttachmentType>();


        }



    }
}
