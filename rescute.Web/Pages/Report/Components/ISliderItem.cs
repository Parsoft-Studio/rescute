using rescute.Web.Pages.Report.ViewModel;

namespace rescute.Web.Pages.Report.Components;

/// <summary>
///     Represents a report item that has a list of slides to show.
/// </summary>
public interface ISliderItem
{
    public IList<ReportItemSlide> Slides { get; set; }
}