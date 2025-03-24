using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Application.Reports;

public interface IReportsService
{
    Task<IReadOnlyList<StatusReport>> GetReportsAsync(GetReportsQuery query);
    int GetPageSize();
}