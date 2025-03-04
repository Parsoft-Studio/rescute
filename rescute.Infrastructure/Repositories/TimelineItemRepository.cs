using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

internal class TimelineItemRepository(rescuteContext context)
    : Repository<TimelineItem>(context), ITimelineItemRepository
{
    public async Task<IReadOnlyList<T>> GetAllAsync<T>()
    {
        return await context.TimelineItems.OfType<T>().ToArrayAsync();
    }

    public async Task<IReadOnlyList<StatusReport>> GetStatusReportsAsync(ITimelineItemRepository.StatusReportFilter filter,
        int pageSize, int pageIndex)
    {
        IQueryable<StatusReport> query = filter.Filter(context.TimelineItems, context.Comments);
        query = query.Skip(pageIndex * pageSize).Take(pageSize);

        return await query.ToArrayAsync();
    }
}