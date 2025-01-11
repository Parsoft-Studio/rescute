using Bogus;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Infrastructure;

namespace rescute.Web.Extensions;

// ReSharper disable once InconsistentNaming
public static class rescuteContextExtensions
{
    private static readonly List<string> Images = ["img/reports/1.jpg", "img/reports/2.jpg"];

    /// <summary>
    /// Fills the provided <see cref="rescuteContext"/> with fake data, useful for testing the application.
    /// </summary>
    /// <param name="context">The <see cref="rescuteContext"/> to fill.</param>
    /// <returns></returns>
    internal static rescuteContext WithFakeData(this rescuteContext context)
    {
        if (!context.Database.EnsureCreated()) return context;
        
        // database just got created, so fill with data.
        
        FillTimelineItems(context);
        
        context.SaveChanges();

        return context;
    }

    private static void FillTimelineItems(rescuteContext context)
    {
        var reportsFaker = new Faker<StatusReport>("fa")
            .CustomInstantiator(f =>
                new StatusReport(f.Date.Recent(),
                    Id<Samaritan>.Generate(),
                    Id<Animal>.Generate(),
                    MapPoint.Empty(),
                    f.Lorem.Paragraph(2),
                    new Attachment(f.Random.ListItem(Images), "jpg", f.Date.Recent(), f.Lorem.Sentence())));

        context.TimelineItems.AddRange(reportsFaker.Generate(100));

    }
}