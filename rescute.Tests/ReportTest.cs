using System;
using Xunit;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using FluentAssertions;
using System.Linq;
using rescute.Domain.Entities.LogItems;
using rescute.Domain.Exceptions;

namespace rescute.Tests
{
    public class ReportTest
    {
        [Fact]
        public void NewReportHasProperReportCreatedLog()
        {
            var report = Report.New(
                creationDate: DateTime.Now,
                createdBy: new Samaritan(),
                location: new MapPoint(10, 20),
                description: string.Empty,
                (Animal)new Animal(AnimalType.Cat));

            report.Logs.Count.Should().Be(1);
            report.Logs.First().GetType().Should().Be(typeof(ReportCreated));
        }
        [Fact]
        public void SamaritanRequestsTransport()
        {
            var samaritan = new Samaritan();
            Animal animal = new Animal(AnimalType.Cat);
            MapPoint point1 = new MapPoint(10, 20);
            MapPoint point2 = new MapPoint(30, 40);
            var date = DateTime.Now;
            var comment = "I just need to get there";


            Report report = Report.New(date, samaritan, point1, string.Empty, animal);
            var inside = report.RequestTransport(samaritan, point1, point2, comment);


            report.Logs.Count().Should().Be(2);
            var outside = report.Logs.Last() as TransportRequested;
            inside.Should().Be(outside);
            outside.Should().NotBeNull();
            outside.Samaritan.Should().Be(samaritan);
            outside.EventLocation.Should().Be(point1);
            outside.ToLocation.Should().Be(point2);
            outside.Description.Should().Be(comment);
        }

        [Fact]
        public void SamaritanAttachesBill()
        {
            var samaritan = new Samaritan();
            var billTotal = 150_000;
            var comment1 = "An animal was hurt here.";
            var comment2 = "I can't pay this on my own!";
            var contributeRequested = true;


            var report = Report.New(
                creationDate: DateTime.Now,
                createdBy: samaritan,
                location: new MapPoint(10, 20),
                description: comment1,
                (Animal)new Animal(AnimalType.Dog));

            var inside = report.AttachBill(samaritan, new Document(DocumentType.Bill, "filename", DateTime.Now), billTotal, contributeRequested, comment2);
            var outside = (BillAttached)(report.Logs.Skip(1).Take(1).First());
            inside.UpdateTotal(1000); // Updating the returned instance.


            report.Logs.Count().Should().Be(2);
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

            var report = Report.New(
                creationDate: DateTime.Now,
                createdBy: samaritan,
                location: new MapPoint(10, 20),
                description: "An animal was hurt here.",
                (Animal)new Animal(AnimalType.Dog));

            var inside = report.AttachBill(
                attachedBy: samaritan,
                bill: new Document(DocumentType.Bill, "filename", DateTime.Now),
                billTotal: 150_000,
                requestingContributions: true,
                comments: "I can't pay this on my own!");

            var contrubution = report.Contribute(inside, contributor, contributionAmount, contribution_comment);

            report.Logs.Count().Should().Be(3);
            contrubution.Should().NotBeNull();
            contrubution.Samaritan.Should().Be(contributor);
            contrubution.Description.Should().Be(contribution_comment);
            contrubution.Amount.Should().Be(contributionAmount);
        }
        [Fact]
        public void ReportDoesntAcceptExcessContribution()
        {
            var samaritan = new Samaritan();
            var contributor = new Samaritan();

            var report = Report.New(
                creationDate: DateTime.Now, 
                createdBy: samaritan, 
                location: new MapPoint(10, 20), 
                description: "An animal was hurt here.",
                (Animal)new Animal(AnimalType.Dog));

            var inside = report.AttachBill(
                attachedBy: samaritan,
                bill: new Document(DocumentType.Bill, "filename", DateTime.Now), 
                billTotal: 150_000, 
                requestingContributions: true, 
                comments: "I can't pay this on my own!");

            var contrubution = report.Contribute(
                bill: inside,
                contributor,
                amount: 100_000,
                comments:"Here, have this contribution.");

            Action action = () =>
            {
                var excess = report.Contribute(
                    bill: inside,
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


            var report = Report.New(
                creationDate: DateTime.Now, 
                createdBy: samaritan, 
                location: new MapPoint(10, 20), 
                description: "An animal was hurt here.",
                (Animal)new Animal(AnimalType.Dog));

            var inside = report.AttachBill(
                attachedBy: samaritan,
                bill: new Document(DocumentType.Bill, "filename", DateTime.Now),
                billTotal: 150_000,
                requestingContributions: true,
                comments: "I can't pay this on my own!");

            var comment = report.Comment(commenter, merly_comment, inside);

            report.Logs.Count().Should().Be(3);
            comment.Should().NotBeNull();
            comment.Samaritan.Should().Be(commenter);
            comment.Description.Should().Be(merly_comment);
            comment.RepliesTo.Should().Be(inside);
        }

        [Fact]

        public void TwoAnimalsOfDifferentTypeAreNotTheSame()
        {
            Animal animal1 = new Animal(AnimalType.Other);
            Animal animal2 = new Animal(AnimalType.Pigeon);

            animal1.Should().NotBe(animal2);
        }

        [Fact]
        public void EventsOnlyAcceptProperDocumentTypes()
        {
            var sam = new Samaritan();
            var docImage = new Document(DocumentType.Image, "filename", DateTime.Now);
            var docVid = new Document(DocumentType.Video, "filename", DateTime.Now);
            var docTest = new Document(DocumentType.TestResult, "filename", DateTime.Now);
            var docBill = new Document(DocumentType.Bill, "filename", DateTime.Now);
            var point = new MapPoint(0, 0);

            // ReportCreated
            var reportCreatedAction = new Action(() =>
            {
                var reportCreated = new ReportCreated(DateTime.Now, sam, point, string.Empty, docImage, docTest, docVid, docBill);
            });

            reportCreatedAction.Should().NotThrow<InvalidDocumentType>();


            // BillAttached
            var billAttachedAction = new Action(() =>
            {
                var billAttached = new BillAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid);
            });

            billAttachedAction.Should().Throw<InvalidDocumentType>();



            // ImageAttached
            var imageAttachedAction = new Action(() =>
            {
                var imageAttached = new ImageAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            imageAttachedAction.Should().Throw<InvalidDocumentType>();

            // VideoAttached
            var videoAttachedAction = new Action(() =>
            {
                var videoAttached = new VideoAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            videoAttachedAction.Should().Throw<InvalidDocumentType>();

            // TestResultAttached
            var testResultAttachedAction = new Action(() =>
            {
                var videoAttached = new TestResultAttached(DateTime.Now, sam, string.Empty, docImage, docTest, docVid, docBill);
            });

            testResultAttachedAction.Should().Throw<InvalidDocumentType>();


        }

    }
}
