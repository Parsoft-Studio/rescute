namespace rescute.Web.Pages.Shared.ViewModels;

/// <summary>
/// Generic view model interface that includes a collection of domain object view models
/// </summary>
/// <typeparam name="TDomainObjectViewModel">The type of domain view models this view model contains</typeparam>
/// <typeparam name="TDomainObject">The domain object type that the view model is mapped to</typeparam>
public interface IViewModel<out TDomainObjectViewModel, in TDomainObject>
    where TDomainObjectViewModel : IDomainObjectViewModel<TDomainObjectViewModel, TDomainObject>
{
    /// <summary>
    ///     Prompts the view model to initialize or re-initialize its state with any state variables that are already set.
    /// </summary>
    Task Refresh();

    /// <summary>
    ///     Returns a value indicating whether this view model has initialized and the UI rendering can proceed.
    /// </summary>
    bool IsInitialized();

    /// <summary>
    /// Collection of items this view model contains
    /// </summary>
    IReadOnlyList<TDomainObjectViewModel> Items { get; }

    /// <summary>
    /// Indicates whether there are more items that can be loaded
    /// </summary>
    bool HasMoreItems { get; }

    /// <summary>
    /// Loads the next page of items
    /// </summary>
    Task LoadNextPage();
}