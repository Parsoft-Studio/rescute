using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Infrastructure;

namespace rescute.Application.Reports;

public class ReportsService(Func<IUnitOfWork> unitOfWorkFactory, IApplicationConfiguration applicationConfiguration)
    : ApplicationServiceBase(unitOfWorkFactory, applicationConfiguration), IReportsService
{
    public async Task<IReadOnlyList<StatusReport>> GetReports(GetReportsQuery request)
    {
        await using var unitOfWork = GetUnitOfWork();
        return await unitOfWork.TimelineItems.GetStatusReports(GetConfiguration().ReportsPageSize, request.PageIndex);
    }
}