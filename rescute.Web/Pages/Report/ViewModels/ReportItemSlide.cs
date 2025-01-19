namespace rescute.Web.Pages.Report.ViewModels;

public class ReportItemSlide(string slideImage, string slideTitle)
{
    public string SlideImage { get; } = slideImage;
    public string SlideTitle { get; } = slideTitle;
}