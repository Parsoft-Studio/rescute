using rescute.Application.Reports;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal class ReportsViewModel(IReportsService reportsService) : IViewModel
{
    private bool isInitialized = false;
    public int PageIndex { get; set; }
    public IReadOnlyList<ReportViewModel> Reports { get; private set; }

    public async Task Initialize()
    {
        isInitialized = false;
        Reports = ReportViewModel.Of(await reportsService.GetReports(new GetReportsQuery(PageIndex)));
        isInitialized = true;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}