namespace rescute.Web.Pages.Shared.ViewModels;

public interface IViewModel
{
    /// <summary>
    ///     Prompts the view model to initialize its state with any state variables that are already set.
    /// </summary>
    Task Initialize();

    /// <summary>
    ///     Returns a value indicating whether this view model has initialized and the UI rendering can proceed.
    /// </summary>
    /// <returns></returns>
    bool IsInitialized();
}