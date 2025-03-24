using Microsoft.Extensions.Localization;
using rescute.Application.Reports;
using rescute.Domain;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Repositories;
using rescute.Web.Authentication;
using rescute.Web.Localization;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal class ReportsViewModel(
    IReportsService reportsService,
    IDateTimeProvider timeProvider,
    ICurrentUserService currentUserService,
    IStringLocalizer<LocalizationResources> localizer) : IViewModel<StatusReportViewModel, StatusReport>
{
    private bool hasFundraiserRequest;
    private bool hasTransportRequest;
    private bool isInitialized;
    private bool usersContributions;
    private bool usersReports;
    private Task? refreshTask;
    public int PageIndex { get; set; }
    public bool HasMoreItems { get; private set; } = true;


    public IReadOnlyList<StatusReportViewModel> Items { get; private set; }

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

    public async Task Refresh()
    {
        // If there's already a refresh in progress, wait for it to complete
        if (refreshTask != null && !refreshTask.IsCompleted)
        {
            await refreshTask;
            return;
        }

        PageIndex = 0;
        var fetchedReports = await reportsService.GetReportsAsync(GetReportsQueryWithFilters());
        Items = StatusReportViewModel.Of(fetchedReports, timeProvider, localizer);
        HasMoreItems = fetchedReports.Count == reportsService.GetPageSize();
        isInitialized = true;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    public async Task LoadNextPage()
    {
        // If there's a refresh in progress, wait for it to complete
        if (refreshTask != null && !refreshTask.IsCompleted) await refreshTask;

        PageIndex += 1;
        List<StatusReportViewModel> result = new();
        result.AddRange(Items);

        var newReports = await reportsService.GetReportsAsync(GetReportsQueryWithFilters());
        result.AddRange(StatusReportViewModel.Of(newReports, timeProvider, localizer));
        Items = result;

        // If fewer items are returned than the page size, there are no more items
        HasMoreItems = newReports.Count == reportsService.GetPageSize();
    }

    private GetReportsQuery GetReportsQueryWithFilters()
    {
        return new GetReportsQuery(PageIndex,
            ITimelineItemRepository.StatusReportFilter.CreateFilter(UsersReports, UsersContributions,
                HasTransportRequest, HasFundraiserRequest, currentUserService.GetCurrentUser()));
    }
}