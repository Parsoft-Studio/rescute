using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using Xunit;

namespace rescute.Tests.DomainTests;

public class AnimalTests
{
    private static readonly DateTime DefaultDate = new(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc);

    [Fact]
    public void TwoAnimalsAreNotTheSame()
    {
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal1 = TestUtility.RandomTestAnimal(samaritan.Id);
        var animal2 = TestUtility.RandomTestAnimal(samaritan.Id);

        animal1.Should().NotBe(animal2);
    }

    [Fact]
    public void AnimalTypeIsSetCorrectlyWhenCreated()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var introducedBy = samaritan.Id;
        var description = "Test description";
        var animalType = AnimalType.Dog();

        // Act
        var animal = new Animal(DefaultDate, introducedBy, description, animalType);

        // Assert
        animal.Type.Should().Be(animalType);
    }

    [Fact]
    public void AcceptableAttachmentTypesReturnsExpectedTypes()
    {
        var animal = TestUtility.RandomTestAnimal(TestUtility.RandomTestSamaritan().Id);

        var expectedTypes = new List<AttachmentType> { AttachmentType.Image() };
        var actualTypes = animal.AcceptableAttachmentTypes.ToList();

        actualTypes.Should().BeEquivalentTo(expectedTypes);
    }

    [Fact]
    public void AddAttachmentsAddsAttachmentsToList()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var attachment1 = new Attachment("image1.jpg", "jpg", DefaultDate, "Test image 1");
        var attachment2 = new Attachment("image2.jpg", "jpg", DefaultDate, "Test image 2");

        // Act
        animal.AddAttachments(attachment1, attachment2);

        // Assert
        animal.Attachments.Count.Should().Be(2);
        animal.Attachments.Should().Contain(attachment1);
        animal.Attachments.Should().Contain(attachment2);
    }

    [Fact]
    public void RemoveAttachmentRemovesAttachmentFromList()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var attachment = TestUtility.RandomTestAttachment();

        animal.AddAttachments(attachment);

        // Act
        animal.RemoveAttachment(attachment);

        // Assert
        animal.Attachments.Should().NotContain(attachment);
    }

    [Fact]
    public void ClearAttachmentsClearsAttachmentsList()
    {
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var attachment1 = TestUtility.RandomTestAttachment();
        var attachment2 = TestUtility.RandomTestAttachment();
        animal.AddAttachments(attachment1, attachment2);

        animal.ClearAttachments();

        animal.Attachments.Should().BeEmpty();
    }

    [Fact]
    public void UpdateRegistrationDateUpdatesRegistrationDatePropertyCorrectly()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var newRegDate = DefaultDate.AddDays(-7);

        // Act
        animal.UpdateRegistrationDate(newRegDate);

        // Assert
        animal.RegistrationDate.Should().Be(newRegDate);
    }

    [Fact]
    public void UpdateDescriptionUpdatesDescriptionPropertyCorrectly()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var newDescription = "This is the new description.";

        // Act
        animal.UpdateDescription(newDescription);

        // Assert
        animal.Description.Should().Be(newDescription);
    }

    [Fact]
    public void UpdateAnimalTypeShouldUpdateTypeProperty()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var newType = AnimalType.Dog();

        // Act
        animal.UpdateAnimalType(newType);

        // Assert
        animal.Type.Should().Be(newType);
    }

    [Fact]
    public void UpdateIntroductionSamaritanUpdatesIntroducedByPropertyCorrectly()
    {
        // Arrange
        var samaritan = TestUtility.RandomTestSamaritan();
        var animal = TestUtility.RandomTestAnimal(samaritan.Id);
        var newSamaritan = TestUtility.RandomTestSamaritan();

        // Act
        animal.UpdateIntroductionSamaritan(newSamaritan.Id);

        // Assert
        animal.IntroducedBy.Should().Be(newSamaritan.Id);
    }
}