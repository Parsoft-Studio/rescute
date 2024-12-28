using System;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;
using rescute.Shared;

namespace rescute.Tests;

internal class TestUtility
{
    private TestUtility()
    {
        // utility class
    }

    public static Samaritan RandomTestSamaritan()
    {
        return new Samaritan(new Name("Test First"), new Name("Test Last"), new PhoneNumber(true, "0912121212"),
            DateTime.MinValue);
    }

    public static Animal RandomTestAnimal(Id<Samaritan> introducedBy)
    {
        return new Animal(DateTime.MinValue, introducedBy, "test animal", AnimalType.Cat());
    }

    public static Attachment RandomTestAttachment()
    {
        var filename = Guid.NewGuid().ToString();
        var extension = "jpg";
        var creationDate = DateTime.MinValue;
        var description = "Test attachment";
        return new Attachment(filename, extension, creationDate, description);
    }

    public static DbContextOptions<rescuteContext> GetTestDatabaseOptions()
    {
        return new DbContextOptionsBuilder<rescuteContext>()
            .UseInMemoryDatabase("rescute")
            .Options;
    }
}