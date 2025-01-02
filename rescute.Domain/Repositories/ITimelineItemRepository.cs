using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Domain.Repositories;

public interface ITimelineItemRepository : IRepository<TimelineItem>
{
    Task<IReadOnlyList<StatusReport>> GetStatusReports(int pageSize, int pageIndex);
}