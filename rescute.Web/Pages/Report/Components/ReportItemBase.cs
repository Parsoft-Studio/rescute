using Microsoft.AspNetCore.Components;
using rescute.Web.Pages.Shared.Model;

namespace rescute.Web.Pages.Report.Components;

public abstract class ReportItemBase : ComponentBase
{
    private readonly Guid guid = Guid.NewGuid();
    public string ComponentId => ReportItemName + "-" + guid;
    /// <summary>
    /// Unique prefix of this report item, which will be used to create a unique component id.
    /// </summary>
    protected abstract string ReportItemName { get; }
    public abstract string ReportItemCaption { get; }
    [Parameter] public Profile Profile { get; set; }
    [Parameter] public string Timestamp { get; set; }
    [Parameter] public string TimestampTitle { get; set; }
    [Parameter] public string Description { get; set; }

}