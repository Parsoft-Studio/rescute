using System.Linq.Expressions;
using Bogus;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Domain.ValueObjects;

namespace rescute.Web.MockData;

public class MockTimelineItemRepository : ITimelineItemRepository
{
    private static readonly List<string> Images = ["img/reports/1.jpg", "img/reports/2.jpg"];
    public Task<TimelineItem> GetAsync(Id<TimelineItem> id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TimelineItem>> GetAsync(Expression<Func<TimelineItem, bool>> predicate, int pageSize,
        int pageIndex)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TimelineItem>> GetAsync(Expression<Func<TimelineItem, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TimelineItem>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public void Add(TimelineItem item)
    {
        throw new NotImplementedException();
    }

    public void Remove(TimelineItem item)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<StatusReport>> GetStatusReports(int pageSize, int pageIndex)
    {
        return await Task.Run(() =>
        {
            var reportsFaker = new Faker<StatusReport>("fa")
                .CustomInstantiator(f => 
                    new StatusReport(f.Date.Recent(),
                    Id<Samaritan>.Generate(),
                    Id<Animal>.Generate(),
                    MapPoint.Empty,
                    f.Lorem.Paragraph(2),
                    new Attachment(f.Random.ListItem(Images), "jpg", f.Date.Recent(), f.Lorem.Sentence())));

            return reportsFaker.Generate(100);
        });
    }
}