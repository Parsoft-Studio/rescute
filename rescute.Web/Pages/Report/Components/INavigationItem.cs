namespace rescute.Web.Pages.Report.Components;

/// <summary>
///     Represents a report item that has a origin and destination point on the map.
/// </summary>
public interface INavigationItem : ICoordinatedItem
{
    public string Destination { get; }
}