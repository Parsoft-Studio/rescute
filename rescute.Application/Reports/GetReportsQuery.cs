using rescute.Domain.Repositories;

namespace rescute.Application.Reports;

public record GetReportsQuery(int PageIndex, ITimelineItemRepository.StatusReportFilter Filter)
{
}