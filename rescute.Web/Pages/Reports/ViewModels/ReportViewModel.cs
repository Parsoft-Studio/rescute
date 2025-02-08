using Microsoft.Extensions.Localization;
using rescute.Domain;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Web.Localization;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal record ReportViewModel(string Image, string Description, string Distance, string Time)
    : IDomainObjectViewModel<ReportViewModel, StatusReport>
{
    private static readonly MapPoint HelsinkiLocation = new(60.1695, 24.9354);

    public static ReportViewModel Of(StatusReport domainObject, IDateTimeProvider timeProvider,
        IStringLocalizer<LocalizationResources> localizer)
    {
        return new ReportViewModel(domainObject.Attachments[0].FileName,
            domainObject.Description,
            GetDistanceText(domainObject.EventLocation.GetDistanceTo(HelsinkiLocation), localizer),
            GetTimeText(domainObject.EventDate, timeProvider, localizer));
    }

    public static IReadOnlyList<ReportViewModel> Of(IReadOnlyList<StatusReport> domainObjects,
        IDateTimeProvider timeProvider, IStringLocalizer<LocalizationResources> localizer)
    {
        return domainObjects.Select(domainObject => Of(domainObject, timeProvider, localizer)).ToArray();
    }

    private static string GetDistanceText(Distance distance, IStringLocalizer<LocalizationResources> localizer)
    {
        return distance.Meters >= 1000
            ? distance.Kilometers + " " + localizer[LocalizationResources.Kilometers]
            : distance.Meters + " " + localizer[LocalizationResources.Meters];
    }

    private static string GetTimeText(DateTime time, IDateTimeProvider timeProvider,
        IStringLocalizer<LocalizationResources> localizer)
    {
        var difference = timeProvider.Now.Subtract(time);
        if (difference.Minutes <= 59) return difference.Minutes + " " + localizer[LocalizationResources.MinutesAgo];
        if (difference.Hours <= 23) return difference.Hours + " " + localizer[LocalizationResources.HoursAgo];
        return difference.Days + " " + localizer[LocalizationResources.DaysAgo];
    }
}