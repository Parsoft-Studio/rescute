using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace rescute.Domain.Repositories
{
    public interface ITimelineEventRepository: IRepository<TimelineEvent>
    {
        Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyCollection<T>> GetAllAsync<T>();
        Task<IReadOnlyCollection<T>> GetAsync<T>(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);

    }
}
