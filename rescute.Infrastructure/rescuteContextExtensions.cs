using System.Collections.Generic;
using System.Linq;
using Bogus;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;

namespace rescute.Infrastructure;

// ReSharper disable once InconsistentNaming
public static class rescuteContextExtensions
{
    private static readonly List<string> Images = ["img/reports/1.jpg", "img/reports/2.jpg"];
    private static readonly MapPoint HelsinkiLocation = new(60.1695, 24.9354);

    /// <summary>
    ///     Fills the provided <see cref="rescuteContext" /> with fake data, useful for testing the application.
    /// </summary>
    /// <param name="context">The <see cref="rescuteContext" /> to fill.</param>
    /// <returns></returns>
    public static rescuteContext WithFakeData(this rescuteContext context)
    {
        if (!context.Database.EnsureCreated()) return context;

        // database just got created, so fill with data.

        FillSamaritans(context);
        FillTimelineItems(context);


        return context;
    }

    private static void FillSamaritans(rescuteContext context)
    {
        var samaritansFaker = new Faker<Samaritan>("en_US")
            .CustomInstantiator(f => new Samaritan(new Name(f.Name.FirstName()), new Name(f.Name.LastName()),
                new PhoneNumber(true, f.Phone.PhoneNumber()), f.Date.Recent()));

        context.Samaritans.AddRange(samaritansFaker.Generate(100));

        context.SaveChanges();
    }

    private static void FillTimelineItems(rescuteContext context)
    {
        var reportsFaker = new Faker<StatusReport>("en_US")
            .CustomInstantiator(f =>
                new StatusReport(f.Date.Recent(2),
                    f.PickRandom(context.Samaritans.AsQueryable().Select(samaritan => samaritan.Id).ToArray()),
                    Id<Animal>.Generate(),
                    new MapPoint(HelsinkiLocation.Latitude + f.Random.Double(-0.2, 0.2),
                        HelsinkiLocation.Longitude + f.Random.Double(-0.2, 0.2)),
                    f.Lorem.Paragraph(2),
                    new Attachment(f.Random.ListItem(Images), "jpg", f.Date.Recent(), f.Lorem.Sentence())));

        context.TimelineItems.AddRange(reportsFaker.Generate(100));

        context.SaveChanges();
    }
}