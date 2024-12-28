namespace rescute.Web.Pages.Report.Components;

/// <summary>
///     Represents a report item that points to a location on the map.
/// </summary>
public interface ICoordinatedItem
{
    /// <summary>
    ///     The name or coordinates of the location of the item on the map
    /// </summary>
    public string Location { get; }

    /// <summary>
    ///     The distance between the user and the location
    /// </summary>
    public string Distance { get; set; }
}