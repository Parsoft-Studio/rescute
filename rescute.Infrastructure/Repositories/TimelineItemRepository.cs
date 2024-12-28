using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories;

public class TimelineItemRepository : Repository<TimelineItem>, ITimelineItemRepository
{
    private readonly rescuteContext context;

    public TimelineItemRepository(rescuteContext c) : base(c)
    {
        context = c;
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync<T>()
    {
        return await context.TimelineItems.OfType<T>().ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate)
    {
        return await context.TimelineItems.OfType<T>().Where(predicate).ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, int pageSize,
        int pageIndex)
    {
        return await context.TimelineItems.OfType<T>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize)
            .ToArrayAsync();
    }
}