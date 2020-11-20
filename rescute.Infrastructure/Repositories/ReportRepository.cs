using System;
using System.Collections.Generic;
using System.Text;
using rescute.Domain.Repositories;
using rescute.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace rescute.Infrastructure.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(rescuteContext c) : base(c)
        {
        }
    }
}
