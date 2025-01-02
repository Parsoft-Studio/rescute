using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using rescute.Web.Pages.Shared.ViewModels;

namespace rescute.Web.Pages.Reports.ViewModels;

internal record ReportViewModel(string Image, string Description, string Distance)
    : IDomainObjectViewModel<ReportViewModel, StatusReport>
{
    private static readonly MapPoint HelsinkiLocation = new(60.1695, 24.9354);

    public static ReportViewModel Of(StatusReport domainObject)
    {
        return new ReportViewModel(domainObject.Attachments[0].FileName, domainObject.Description,
            GetDistanceText(domainObject.EventLocation.GetDistanceTo(HelsinkiLocation)));
    }

    public static IReadOnlyList<ReportViewModel> Of(IReadOnlyList<StatusReport> domainObjects)
    {
        return domainObjects.Select(Of).ToArray();
    }

    private static string GetDistanceText(Distance distance)
    {
        return (distance.Meters >= 1000 ? distance.Kilometers + " " + "کیلومتر" : distance.Meters + " " + "متر");
    }
}