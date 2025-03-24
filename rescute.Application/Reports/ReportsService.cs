using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Domain.ValueObjects;

namespace rescute.Application.Reports;

public class ReportsService(Func<IUnitOfWork> unitOfWorkFactory, IApplicationConfiguration applicationConfiguration)
    : ApplicationServiceBase(unitOfWorkFactory, applicationConfiguration), IReportsService
{
    public async Task<IReadOnlyList<StatusReport>> GetReportsAsync(GetReportsQuery query)
    {
        await using var unitOfWork = GetUnitOfWork();
        return await unitOfWork.TimelineItems.GetStatusReportsAsync(query.Filter,
            GetConfiguration().ReportsPageSize,
            query.PageIndex);
    }

    public int GetPageSize()
    {
        return GetConfiguration().ReportsPageSize;
    }
}