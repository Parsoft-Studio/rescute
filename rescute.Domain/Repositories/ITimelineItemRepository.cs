using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Domain.Repositories;

public interface ITimelineItemRepository : IRepository<TimelineItem>
{
    Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);
    Task<IReadOnlyCollection<T>> GetAllAsync<T>();
}