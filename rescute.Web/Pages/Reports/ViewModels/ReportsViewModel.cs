using Microsoft.Extensions.Localization;
using rescute.Application.Reports;
using rescute.Domain;
using rescute.Domain.Repositories;
using rescute.Web.Authentication;
using rescute.Web.Localization;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal class ReportsViewModel(
    IReportsService reportsService,
    IDateTimeProvider timeProvider,
    ICurrentUserService currentUserService,
    IStringLocalizer<LocalizationResources> localizer) : IViewModel
{
    private bool isInitialized;
    private bool usersReports;
    private bool usersContributions;
    private bool hasTransportRequest;
    private bool hasFundraiserRequest;
    private Task? refreshTask;
    public int PageIndex { get; set; }

    public bool UsersReports
    {
        get => usersReports;
        set
        {
            if (usersReports == value) return;
            usersReports = value;
            refreshTask = Refresh();
        }
    }

    public bool UsersContributions
    {
        get => usersContributions;
        set
        {
            if (usersContributions == value) return;
            usersContributions = value;
            refreshTask = Refresh();
        }
    }

    public bool HasTransportRequest
    {
        get => hasTransportRequest;
        set
        {
            if (hasTransportRequest == value) return;
            hasTransportRequest = value;
            refreshTask = Refresh();
        }
    }

    public bool HasFundraiserRequest
    {
        get => hasFundraiserRequest;
        set
        {
            if (hasFundraiserRequest == value) return;
            hasFundraiserRequest = value;
            refreshTask = Refresh();
        }
    }

    public IReadOnlyList<ReportViewModel> Reports { get; private set; } = new List<ReportViewModel>();

    public async Task Refresh()
    {
        // If there's already a refresh in progress, wait for it to complete
        if (refreshTask != null && !refreshTask.IsCompleted)
        {
            await refreshTask;
            return;
        }

        PageIndex = 0;
        Reports = ReportViewModel.Of(await reportsService.GetReportsAsync(GetReportsQueryWithFilters()),
            timeProvider,
            localizer);
        isInitialized = true;
    }

    public async Task LoadNextPage()
    {
        // If there's a refresh in progress, wait for it to complete
        if (refreshTask != null && !refreshTask.IsCompleted)
        {
            await refreshTask;
        }

        PageIndex += 1;
        List<ReportViewModel> result = new();
        result.AddRange(Reports);
        result.AddRange(ReportViewModel.Of(
            await reportsService.GetReportsAsync(GetReportsQueryWithFilters()), timeProvider,
            localizer));
        Reports = result;
    }

    private GetReportsQuery GetReportsQueryWithFilters()
    {
        return new GetReportsQuery(PageIndex,
            ITimelineItemRepository.StatusReportFilter.CreateFilter(UsersReports, UsersContributions,
                HasTransportRequest, HasFundraiserRequest, currentUserService.GetCurrentUser()));
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
}
