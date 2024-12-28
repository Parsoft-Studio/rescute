using Microsoft.AspNetCore.Components;
using rescute.Web.Pages.Shared.ViewModel;

namespace rescute.Web.Pages.Report.Components;

public abstract class ReportItemBase : ComponentBase
{
    private readonly Guid guid = Guid.NewGuid();
    public string ComponentId => ReportItemName + "-" + guid;

    /// <summary>
    ///     Unique prefix of this report item, which will be used to create a unique component id.
    /// </summary>
    protected abstract string ReportItemName { get; }

    public abstract string ReportItemCaption { get; }
    [Parameter] public required Profile Profile { get; set; }
    [Parameter] public required string Timestamp { get; set; }
    [Parameter] public required string TimestampTitle { get; set; }
    [Parameter] public required string Description { get; set; }
}