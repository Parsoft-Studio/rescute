using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using rescute.Domain;
using rescute.Domain.ValueObjects;
using rescute.Web.Localization;

namespace rescute.Web.Pages.Shared.ViewModels;

public static class Utility
{
    public static string GetDistanceText(Distance distance, IStringLocalizer<LocalizationResources> localizer)
    {
        return distance.Meters >= 1000
            ? distance.Kilometers + " " + localizer[LocalizationResources.Kilometers]
            : distance.Meters + " " + localizer[LocalizationResources.Meters];
    }

    public static string GetTimeText(DateTime time, IDateTimeProvider timeProvider,
        IStringLocalizer<LocalizationResources> localizer)
    {
        var difference = timeProvider.Now.Subtract(time);
        if (difference.Minutes <= 59) return difference.Minutes + " " + localizer[LocalizationResources.MinutesAgo];
        if (difference.Hours <= 23) return difference.Hours + " " + localizer[LocalizationResources.HoursAgo];
        return difference.Days + " " + localizer[LocalizationResources.DaysAgo];
    }
}