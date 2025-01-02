using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;

namespace rescute.Application.Reports;

public class ReportsService(ITimelineItemRepository timelineItemRepository) : IReportsService
{
    private const int PageSize = 10;

    public async Task<IReadOnlyList<StatusReport>> GetReports(GetReportsQuery request)
    {
        return await timelineItemRepository.GetStatusReports(PageSize, request.PageIndex);
    }
}