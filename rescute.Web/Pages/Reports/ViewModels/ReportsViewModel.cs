using Microsoft.Extensions.Localization;
using rescute.Application.Reports;
using rescute.Domain;
using rescute.Web.Localization;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal class ReportsViewModel(
    IReportsService reportsService,
    IDateTimeProvider timeProvider,
    IStringLocalizer<LocalizationResources> localizer) : IViewModel
{
    private bool isInitialized;
    public int PageIndex { get; set; }
    public IReadOnlyList<ReportViewModel> Reports { get; private set; }

    public async Task Initialize()
    {
        Reports = ReportViewModel.Of(await reportsService.GetReports(new GetReportsQuery(PageIndex)), timeProvider,
            localizer);
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
            timeProvider, localizer));
        Reports = result;
    }
}