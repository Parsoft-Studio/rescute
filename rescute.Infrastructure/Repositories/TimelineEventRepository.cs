using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories
{
    public class TimelineEventRepository : Repository<TimelineEvent>, ITimelineEventRepository
    {
        private readonly rescuteContext context;
        public TimelineEventRepository(rescuteContext c) : base(c)
        {
            context = c;
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync<T>()
        {
            return await context.TimelineEvents.OfType<T>().ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate)
        {
            return await context.TimelineEvents.OfType<T>().Where(predicate).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex)
        {
            return await context.TimelineEvents.OfType<T>().Where(predicate).Skip(pageIndex * pageSize).Take(pageSize).ToArrayAsync();
        }
    }
}
