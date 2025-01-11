using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

internal class TimelineItemRepository : Repository<TimelineItem>, ITimelineItemRepository
{
    private readonly rescuteContext context;

    public TimelineItemRepository(rescuteContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync<T>()
    {
        return await context.TimelineItems.OfType<T>().ToArrayAsync();
    }
    
    public async Task<IReadOnlyList<StatusReport>> GetStatusReports(int pageSize, int pageIndex)
    {
        return await context.TimelineItems.OfType<StatusReport>().Skip(pageIndex * pageSize).Take(pageSize)
            .ToArrayAsync();
    }
}