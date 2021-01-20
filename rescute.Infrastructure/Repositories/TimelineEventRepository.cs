using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.Repositories;

namespace rescute.Infrastructure.Repositories
{
    public class TimelineEventRepository : Repository<TimelineEvent>, ITimelineEventRepository
    {
        public TimelineEventRepository(rescuteContext c) : base(c)
        {
        }
    }
}
