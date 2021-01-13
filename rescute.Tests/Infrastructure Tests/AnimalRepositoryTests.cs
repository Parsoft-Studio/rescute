using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.Entities.LogItems;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace rescute.Tests.InfrastructureTests
{
    [Collection("Database collection")]
    public class AnimalRepositoryTests
    {

        [Fact]
        public async void AnimalRepositoryAddsAndGetsAnimal()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Ehsan"), new Name("Hosseinkhani"), new PhoneNumber(true, "09355242601"));
                    var animal = Animal.New(DateTime.Now, samaritan, "This is my good pet.", AnimalType.Cat);
                    animal.UpdateBirthCertificateId("birth_cert_id");

                    unitOfWork.Animals.Add(animal);
                    await unitOfWork.Complete();
                    var same = await unitOfWork.Animals.GetAsync(animal.Id);


                    same.Should().NotBe(null);
                    same.Should().Be(animal);
                }
            }
        }
        [Fact]
        public async void AnimalRepositoryStoresAnimalAndLogWithAttachment()
        {
            using (var context = new rescuteContext(TestDatabaseInitializer.TestsConnectionString))
            {
                using (var unitOfWork = new UnitOfWork(context))
                {
                    var samaritan = new Samaritan(new Name("Pooya"), new Name("Bisadi"), new PhoneNumber(true, "09385242601"));
                    var animal = Animal.New(DateTime.Now, samaritan, "I found it on the road.", AnimalType.Dog);
                    animal.UpdateBirthCertificateId("birth_cert_id");


                    var status = animal.ReportStatus(
                        new MapPoint(10, 20),
                        samaritan,
                        "The status of the animal is not good.",
                        new Attachment(
                            AttachmentType.Image,
                            "testfilename.jpg",
                            DateTime.Now,
                            "Picture of the dog."));

                    unitOfWork.Animals.Add(animal);
                    await unitOfWork.Complete();
                    var same = await unitOfWork.Animals.GetAsync(animal.Id);


                    same.Should().NotBe(null);
                    same.Should().Be(animal);
                    same.Log.Count.Should().Be(1);
                    same.Log.First().Id.Should().Be(status.Id);
                    ((StatusReported)(same.Log.First())).Attachments.Count.Should().Be(1);
                }
            }
        }

    }
}
