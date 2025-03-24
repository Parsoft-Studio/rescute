using Microsoft.Extensions.Localization;
using rescute.Domain;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Web.Localization;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal record StatusReportViewModel(string Image, string Description, string Distance, string Time)
    : IDomainObjectViewModel<StatusReportViewModel, StatusReport>
{
    private static readonly MapPoint HelsinkiLocation = new(60.1695, 24.9354);

    public static StatusReportViewModel Of(StatusReport domainObject, IDateTimeProvider timeProvider,
        IStringLocalizer<LocalizationResources> localizer)
    {
        return new StatusReportViewModel(domainObject.Attachments[0].FileName,
            domainObject.Description,
            Utility.GetDistanceText(domainObject.EventLocation.GetDistanceTo(HelsinkiLocation), localizer),
            Utility.GetTimeText(domainObject.EventDate, timeProvider, localizer));
    }

    public static IReadOnlyList<StatusReportViewModel> Of(IReadOnlyList<StatusReport> domainObjects,
        IDateTimeProvider timeProvider, IStringLocalizer<LocalizationResources> localizer)
    {
        return domainObjects.Select(domainObject => Of(domainObject, timeProvider, localizer)).ToArray();
    }

}