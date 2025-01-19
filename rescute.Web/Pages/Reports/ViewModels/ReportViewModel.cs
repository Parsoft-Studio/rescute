using rescute.Domain;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal record ReportViewModel(string Image, string Description, string Distance, string Time)
    : IDomainObjectViewModel<ReportViewModel, StatusReport>
{
    private static readonly MapPoint HelsinkiLocation = new(60.1695, 24.9354);

    public static ReportViewModel Of(StatusReport domainObject, IDateTimeProvider timeProvider)
    {
        return new ReportViewModel(domainObject.Attachments[0].FileName,
            domainObject.Description,
            GetDistanceText(domainObject.EventLocation.GetDistanceTo(HelsinkiLocation)),
            GetTimeText(domainObject.EventDate, timeProvider));
    }

    public static IReadOnlyList<ReportViewModel> Of(IReadOnlyList<StatusReport> domainObjects,
        IDateTimeProvider timeProvider)
    {
        return domainObjects.Select(domainObject => Of(domainObject, timeProvider)).ToArray();
    }

    private static string GetDistanceText(Distance distance)
    {
        return distance.Meters >= 1000 ? distance.Kilometers + " " + "کیلومتر" : distance.Meters + " " + "متر";
    }

    private static string GetTimeText(DateTime time, IDateTimeProvider timeProvider)
    {
        var difference = timeProvider.Now.Subtract(time);
        if (difference.Minutes <= 59) return difference.Minutes + " دقیقه پیش";
        if (difference.Hours <= 23) return difference.Hours + " ساعت پیش";
        return difference.Days + difference.Hours + " روز پیش";
    }
}