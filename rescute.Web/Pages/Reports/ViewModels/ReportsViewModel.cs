using rescute.Application.Reports;
using rescute.Domain;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal class ReportsViewModel(IReportsService reportsService, IDateTimeProvider timeProvider) : IViewModel
{
    private bool isInitialized;
    public int PageIndex { get; set; }
    public IReadOnlyList<ReportViewModel> Reports { get; private set; }

    public async Task Initialize()
    {
        isInitialized = false;
        Reports = ReportViewModel.Of(await reportsService.GetReports(new GetReportsQuery(PageIndex)), timeProvider);
        isInitialized = true;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    public async Task LoadNextPage()
    {
        PageIndex += 1;
        List<ReportViewModel> result = new();
        result.AddRange(Reports);
        result.AddRange(ReportViewModel.Of(await reportsService.GetReports(new GetReportsQuery(PageIndex)),
            timeProvider));
        Reports = result;
    }
}