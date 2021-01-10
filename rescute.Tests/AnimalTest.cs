using System;
using Xunit;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using FluentAssertions;
using System.Linq;
using rescute.Domain.Entities.LogItems;
using rescute.Domain.Exceptions;
using System.Collections.Generic;

namespace rescute.Tests
{
    public class AnimalTest
    {
        [Fact]
        public void SamaritanReportsAnimalAndStatus()
        {
            var samaritan = new Samaritan();
            var desc = "this is a status descritpion";
            var location = new MapPoint(10, 20);
            var animal = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: string.Empty,
                AnimalType.Cat);

            var docs = new List<Attachment>();
            docs.Add(new Attachment(AttachmentType.Image, "filename1", DateTime.Now, string.Empty));
            docs.Add(new Attachment(AttachmentType.Video, "filename2", DateTime.Now.AddSeconds(4), string.Empty));

            var inner = animal.ReportStatus(location, samaritan, desc, docs.ToArray());

            animal.Log.First().GetType().Should().Be(typeof(StatusReported));
            animal.Log.First().Should().Be(inner);
            inner.Description.Should().Be(desc);
            inner.EventLocation.Should().Be(location);            
            inner.Attachments.Count.Should().Be(2);
        }
        [Fact]
        public void SamaritanRequestsTransport()
        {
            var samaritan = new Samaritan();

            MapPoint point1 = new MapPoint(10, 20);
            MapPoint point2 = new MapPoint(30, 40);
            var date = DateTime.Now;
            var comment1 = "There is a cute animal in need";
            var comment2 = "I need to get it there";

            Animal animal = Animal.New(
                registrationDate: date,
                introducedBy: samaritan,
                description: comment1,
                type: AnimalType.Cat);

            var inside = animal.RequestTransport(samaritan, point1, point2, comment2);


            animal.Log.Count().Should().Be(1);
            var outside = animal.Log.Last() as TransportRequested;
            inside.Should().Be(outside);
            outside.Should().NotBeNull();
            outside.Samaritan.Should().Be(samaritan);
            outside.EventLocation.Should().Be(point1);
            outside.ToLocation.Should().Be(point2);
            outside.Description.Should().Be(comment2);
        }

        [Fact]
        public void SamaritanAttachesBill()
        {
            var samaritan = new Samaritan();
            var billTotal = 150_000;
            var comment1 = "An animal was hurt here.";
            var comment2 = "I can't pay this on my own!";
            var contributeRequested = true;


            var animal = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: comment1,
                type: AnimalType.Dog);

            var inside = animal.AttachBill(samaritan, new Attachment(AttachmentType.Bill, "filename", DateTime.Now, string.Empty), billTotal, contributeRequested, comment2);
            var outside = (BillAttached)(animal.Log.First());
            inside.UpdateTotal(1000); // Updating the returned instance.


            animal.Log.Count().Should().Be(1);
            outside.Should().NotBeNull();
            outside.Samaritan.Should().Be(samaritan);
            outside.Description.Should().Be(comment2);
            outside.Total.Should().Be(billTotal);
            outside.ContributionRequested.Should().Be(contributeRequested);
            inside.Should().Be(outside);   // Even though the two instance have the same ID and therefore are considered equal...
            outside.Total.Should().Be(billTotal); // ...report's own instance should not be modified by the other instance.
        }

        [Fact]
        public void SamaritanContributesToBill()
        {

            var contribution_comment = "Here, have this contribution.";
            var contributionAmount = 100000;

            var samaritan = new Samaritan();
            var contributor = new Samaritan();

            var animal = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: "this is an animal",
                type: AnimalType.Dog);

            var bill = animal.AttachBill(
                attachedBy: samaritan,
                bill: new Attachment(AttachmentType.Bill, "filename", DateTime.Now, string.Empty),
                billTotal: 150_000,
                requestingContributions: true,
                comments: "I can't pay this on my own!");

            var contrubution = animal.SamaritanContributes(bill, contributor, contributionAmount, contribution_comment);

            animal.Log.Count().Should().Be(2);
            contrubution.Should().NotBeNull();
            contrubution.Samaritan.Should().Be(contributor);
            contrubution.Description.Should().Be(contribution_comment);
            contrubution.Amount.Should().Be(contributionAmount);
        }
        [Fact]
        public void AnimalDoesntAcceptExcessContribution()
        {
            var samaritan = new Samaritan();
            var contributor = new Samaritan();

            var animal = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: "this is an animal",
                type: AnimalType.Pigeon);

            var bill = animal.AttachBill(
                attachedBy: samaritan,
                bill: new Attachment(AttachmentType.Bill, "filename", DateTime.Now, string.Empty),
                billTotal: 150_000,
                requestingContributions: true,
                comments: "I can't pay this on my own!");

            var contrubution = animal.SamaritanContributes(
                bill: bill,
                contributor,
                amount: 100_000,
                comments: "Here, have this contribution.");

            Action action = () =>
            {
                var excess = animal.SamaritanContributes(
                    bill: bill,
                    contributor,
                    amount: 51_000,
                    comments: "Maybe a little too much.");
            };
            action.Should().Throw<ContributionExceedsRequirement>();
        }

        [Fact]
        public void SamaritanCommentsOnLogItem()
        {
            var samaritan = new Samaritan();
            var commenter = new Samaritan();
            var merly_comment = "Haha! That's funny!";


            var animal = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: "this is an animal",
                type: AnimalType.Dog);

            var bill = animal.AttachBill(
                attachedBy: samaritan,
                bill: new Attachment(AttachmentType.Bill, "filename", DateTime.Now, string.Empty),
                billTotal: 150_000,
                requestingContributions: true,
                comments: "I can't pay this on my own!");

            var comment = animal.SamaritanComments(commenter, merly_comment, bill);

            animal.Log.Count().Should().Be(2);
            comment.Should().NotBeNull();
            comment.Samaritan.Should().Be(commenter);
            comment.Description.Should().Be(merly_comment);
            comment.RepliesTo.Should().Be(bill);
        }

        [Fact]

        public void TwoAnimalsOfDifferentTypeAreNotTheSame()
        {
            var samaritan = new Samaritan();
            var animal1 = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: "this is an animal",
                type: AnimalType.Sparrow);

            var animal2 = Animal.New(
                registrationDate: DateTime.Now,
                introducedBy: samaritan,
                description: "this is an animal",
                type: AnimalType.Sparrow);

            animal1.Should().NotBe(animal2);
        }

        [Fact]
        public void EventsOnlyAcceptProperDocumentTypes()
        {
            var sam = new Samaritan();
            var docImage = new Attachment(AttachmentType.Image, "filename", DateTime.Now, string.Empty);
            var docVid = new Attachment(AttachmentType.Video, "filename", DateTime.Now, string.Empty);
            var docTest = new Attachment(AttachmentType.TestResult, "filename", DateTime.Now, string.Empty);
            var docBill = new Attachment(AttachmentType.Bill, "filename", DateTime.Now, string.Empty);
            var point = new MapPoint(0, 0);

            // StatusReported
            var statusReportedAction = new Action(() =>
            {
                var statusReported = new StatusReported(DateTime.Now, sam, point, string.Empty, docImage, docVid);
            });

            statusReportedAction.Should().NotThrow<InvalidAttachmentType>();


            // BillAttached
            var billAttachedAction = new Action(() =>
            {
                var billAttached = new BillAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid);
            });

            billAttachedAction.Should().Throw<InvalidAttachmentType>();



            // ImageAttached
            var imageAttachedAction = new Action(() =>
            {
                var imageAttached = new ImageAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            imageAttachedAction.Should().Throw<InvalidAttachmentType>();

            // VideoAttached
            var videoAttachedAction = new Action(() =>
            {
                var videoAttached = new VideoAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            videoAttachedAction.Should().Throw<InvalidAttachmentType>();

            // TestResultAttached
            var testResultAttachedAction = new Action(() =>
            {
                var videoAttached = new TestResultAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            testResultAttachedAction.Should().Throw<InvalidAttachmentType>();


        }

    }
}
