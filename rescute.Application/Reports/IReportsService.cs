using rescute.Domain.Aggregates.TimelineItems;

namespace rescute.Application.Reports;

public interface IReportsService
{
    Task<IReadOnlyList<StatusReport>> GetReports(GetReportsQuery request);
}